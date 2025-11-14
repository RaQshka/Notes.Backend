using Notes.Domain;

namespace Notes.Application.Interfaces;


public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string ipAddress);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token, string ipAddress, string reason = "Revoked without replacement");
    Task RevokeDescendantRefreshTokensAsync(RefreshToken refreshToken, string ipAddress, string reason);
    Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken, string ipAddress);
}