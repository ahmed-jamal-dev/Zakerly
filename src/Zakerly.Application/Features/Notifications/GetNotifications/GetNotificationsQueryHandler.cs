using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Notifications.GetNotifications;

public class GetNotificationsQueryHandler
    : IRequestHandler<GetNotificationsQuery, PagedResponse<NotificationResponse>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetNotificationsQueryHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResponse<NotificationResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var notifications = await _notificationRepository.GetByRecipientIdPagedAsync(
            userId,
            request.Pagination.Skip,
            request.Pagination.Take,
            cancellationToken);

        var totalCount = await _notificationRepository.CountByRecipientIdAsync(
            userId,
            cancellationToken);

        var items = notifications.Select(n => new NotificationResponse(
            n.Id,
            n.Type,
            n.Title,
            n.Message,
            n.RelatedEntityId,
            n.RelatedEntityType,
            n.IsRead,
            n.CreatedAt,
            n.ReadAt)).ToList();

        var metadata = request.Pagination.CreateMetadata(totalCount);

        return new PagedResponse<NotificationResponse>(items, metadata);
    }
}