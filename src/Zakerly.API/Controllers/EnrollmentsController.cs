using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Features.Enrollments.GetMyEnrollments;

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
}