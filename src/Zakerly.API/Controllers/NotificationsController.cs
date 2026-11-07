using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Common.Pagination;
using Zakerly.Application.Features.Notifications.GetNotifications;
using Zakerly.Application.Features.Notifications.GetUnreadCount;
using Zakerly.Application.Features.Notifications.MarkAsRead;
using Zakerly.Application.Features.Notifications.MarkAllAsRead;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/v1/notifications
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetNotificationsQuery(pagination),
            cancellationToken);

        return Ok(result);
    }

    // GET: api/v1/notifications/unread-count
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUnreadCountQuery(),
            cancellationToken);

        return Ok(new { UnreadCount = result });
    }

    // PATCH: api/v1/notifications/{notificationId}/read
    [HttpPatch("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new MarkAsReadCommand(notificationId),
            cancellationToken);

        return NoContent();
    }

    // PATCH: api/v1/notifications/read-all
    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(
        CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(
            new MarkAllAsReadCommand(),
            cancellationToken);

        return Ok(new { MarkedAsReadCount = count });
    }
}