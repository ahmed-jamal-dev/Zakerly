using Zakerly.Domain.Enums;

namespace Zakerly.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }

    UserRole Role { get; }

    bool IsAuthenticated { get; }
}