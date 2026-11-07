using MediatR;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Enums;

namespace Zakerly.Application.Features.Notifications.GetNotifications;

public sealed record GetNotificationsQuery(
    PaginationParams Pagination)
    : IRequest<PagedResponse<NotificationResponse>>;

public sealed record NotificationResponse(
    Guid Id,
    NotificationType Type,
    string Title,
    string Message,
    Guid? RelatedEntityId,
    string? RelatedEntityType,
    bool IsRead,
    DateTime CreatedAt,
    DateTime? ReadAt);