using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Features.Enrollments.GetMyEnrollments;
using Zakerly.Application.Features.Enrollments.CancelEnrollment;
namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/v1/enrollments/my
    [Authorize(Roles = "Student")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrollments(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMyEnrollmentsQuery(),
            cancellationToken);

        return Ok(result);
    }
    // DELETE: api/v1/enrollments/{enrollmentId}
    [Authorize(Roles = "Student")]
    [HttpDelete("{enrollmentId:guid}")]
    public async Task<IActionResult> CancelEnrollment(
        Guid enrollmentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new CancelEnrollmentCommand(enrollmentId),
            cancellationToken);

        return NoContent();
    }
}