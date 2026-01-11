using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public RefreshTokenCommandHandler(
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService,
        UserManager<User> userManager, 
        ICurrentUserService currentUserService)
    {
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
            return Result<RefreshTokenResponse>.Failure("Invalid token");

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userGuid))
            return Result<RefreshTokenResponse>.Failure("Invalid token");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<RefreshTokenResponse>.Failure("User not found");

        var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || refreshToken.UserId != userGuid || !refreshToken.IsActive)
            return Result<RefreshTokenResponse>.Failure("Invalid refresh token");

        var ipAddress = _currentUserService.GetIpAddress();
        
        var newRefreshToken = await _refreshTokenService.RotateRefreshTokenAsync(refreshToken, ipAddress);
        
        await _refreshTokenService.RevokeDescendantRefreshTokensAsync(refreshToken, ipAddress, 
            $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");

        var newJwtToken = _tokenService.GenerateToken(user);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
        {
            Token = newJwtToken,
            Expiration = DateTime.UtcNow.AddMinutes(15), 
            RefreshToken = newRefreshToken.Token
        });
    }
}