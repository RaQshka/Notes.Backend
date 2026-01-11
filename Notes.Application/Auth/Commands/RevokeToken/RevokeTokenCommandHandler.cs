using MediatR;
using Notes.Application.Common.Models;
using Notes.Application.Interfaces;

namespace Notes.Application.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ICurrentUserService _currentUserService;

    public RevokeTokenCommandHandler(IRefreshTokenService refreshTokenService, 
        ICurrentUserService currentUserService)
    {
        _refreshTokenService = refreshTokenService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken is not { IsActive: true })
            return Result<bool>.Failure("Invalid refresh token");

        var ipAddress = _currentUserService.GetIpAddress();
        
        await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken, ipAddress, "Revoked by user");
        return Result<bool>.Success(true);
    }
}