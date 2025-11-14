using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Persistence.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly NotesDbContext _context;
    private readonly IConfiguration _configuration;

    public RefreshTokenService(NotesDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = await GenerateUniqueToken(),
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpireDays"] ?? "7")),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task RevokeRefreshTokenAsync(string token, string ipAddress, string reason = "Revoked without replacement")
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        if (refreshToken == null || !refreshToken.IsActive) return;

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;

        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeDescendantRefreshTokensAsync(RefreshToken refreshToken, string ipAddress, string reason)
    {
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;

        var childToken = await GetRefreshTokenAsync(refreshToken.ReplacedByToken);
        if (childToken == null || !childToken.IsActive) return;

        childToken.Revoked = DateTime.UtcNow;
        childToken.RevokedByIp = ipAddress;
        childToken.ReasonRevoked = reason;

        _context.RefreshTokens.Update(childToken);
        await _context.SaveChangesAsync();

        // Recursively revoke descendants
        await RevokeDescendantRefreshTokensAsync(childToken, ipAddress, reason);
    }

    public async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = await GenerateRefreshTokenAsync(refreshToken.UserId, ipAddress);
            
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReplacedByToken = newRefreshToken.Token;
        refreshToken.ReasonRevoked = "Replaced by new token";

        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();

        return newRefreshToken;
    }

    private async Task<string> GenerateUniqueToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            
        var tokenExists = await _context.RefreshTokens.AnyAsync(rt => rt.Token == token);
        if (tokenExists)
            return await GenerateUniqueToken();
            
        return token;
    }
}