using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Assignments;
using Zakerly.Application.Features.Assignments.CreateAssignment;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/assignments")]
public class AssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssignmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid courseId,
        [FromBody] CreateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAssignmentCommand(
            courseId,
            request.Title,
            request.Description);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/assignments/{response.AssignmentId}",
            response);
    }
}