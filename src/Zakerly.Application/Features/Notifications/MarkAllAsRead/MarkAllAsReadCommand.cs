using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Notifications.MarkAllAsRead;

public sealed record MarkAllAsReadCommand() : IRequest<int>;

public class MarkAllAsReadCommandHandler : IRequestHandler<MarkAllAsReadCommand, int>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkAllAsReadCommandHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(
        MarkAllAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var count = await _notificationRepository.MarkAllAsReadAsync(
            userId,
            cancellationToken);

        return count;
    }
}