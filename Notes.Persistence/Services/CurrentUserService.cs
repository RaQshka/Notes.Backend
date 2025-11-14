using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Notes.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Notes.Persistence.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?
        .FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?
        .Identity?.IsAuthenticated ?? false;
}