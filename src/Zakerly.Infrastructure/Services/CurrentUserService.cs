using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Enums;

namespace Zakerly.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }

    public UserRole Role
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Role)?
                .Value;

            return Enum.TryParse<UserRole>(value, out var role)
                ? role
                : default;
        }
    }
}