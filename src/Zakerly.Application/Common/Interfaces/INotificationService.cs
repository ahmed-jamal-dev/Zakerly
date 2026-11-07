using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;

namespace Zakerly.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken = default);
}