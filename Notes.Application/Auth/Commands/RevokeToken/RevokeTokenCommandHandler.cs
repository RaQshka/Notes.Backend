using MediatR;
using Notes.Application.Common.Models;
using Notes.Application.Interfaces;

namespace Notes.Application.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public RevokeTokenCommandHandler(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || !refreshToken.IsActive)
            return Result<bool>.Failure("Invalid refresh token");

        await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken, request.IpAddress, "Revoked by user");
        return Result<bool>.Success(true);
    }
}