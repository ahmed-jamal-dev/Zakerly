using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Zakerly.Application.Common.DTOs;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Infrastructure.Hubs;

namespace Zakerly.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IHubContext<NotificationHub> hubContext,
        ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        var signalREvent = new NotificationSignalREvent(
            notification.Id,
            notification.Type,
            notification.Title,
            notification.Message,
            notification.RelatedEntityId,
            notification.RelatedEntityType,
            notification.CreatedAt,
            notification.IsRead);

        try
        {
            await _hubContext.Clients.Group($"user-{notification.RecipientUserId}")
                .SendAsync("ReceiveNotification", signalREvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to deliver real-time notification {NotificationId} to user {UserId}. Notification persisted.",
                notification.Id, notification.RecipientUserId);
        }
    }
}