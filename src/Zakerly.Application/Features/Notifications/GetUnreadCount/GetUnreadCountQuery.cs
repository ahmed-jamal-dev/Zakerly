using MediatR;

namespace Zakerly.Application.Features.Notifications.GetUnreadCount;

public sealed record GetUnreadCountQuery() : IRequest<int>;