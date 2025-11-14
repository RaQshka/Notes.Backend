using System.Security.Claims;
using Notes.Domain;

namespace Notes.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}