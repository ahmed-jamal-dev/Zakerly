using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Notifications.MarkAsRead;

public sealed record MarkAsReadCommand(Guid NotificationId) : IRequest;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkAsReadCommandHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        MarkAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var notification = await _notificationRepository.GetByIdAsync(
            request.NotificationId,
            cancellationToken);

        if (notification is null)
        {
            throw new NotFoundException(nameof(Notification), request.NotificationId);
        }

        if (notification.RecipientUserId != userId)
        {
            throw new ForbiddenException("You are not allowed to mark this notification as read.");
        }

        notification.MarkAsRead();

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}