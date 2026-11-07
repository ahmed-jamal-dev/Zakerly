using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Notifications.GetUnreadCount;

public class GetUnreadCountQueryHandler
    : IRequestHandler<GetUnreadCountQuery, int>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUnreadCountQueryHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(
        GetUnreadCountQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var count = await _notificationRepository.CountUnreadByRecipientIdAsync(
            userId,
            cancellationToken);

        return (int)count;
    }
}