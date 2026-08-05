using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Assignments;
using Zakerly.Application.Features.Assignments.CreateAssignment;
using Zakerly.Application.Features.Assignments.GetAllAssignments;
using Zakerly.Application.Features.Assignments.GetAssignmentById;
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
    // GET: api/v1/courses/{courseId}/assignments
    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllAssignmentsQuery(courseId),
            cancellationToken);

        return Ok(result);
    }
    // GET: api/v1/assignments/{assignmentId}
    [HttpGet("/api/v1/assignments/{assignmentId:guid}")]
    public async Task<IActionResult> GetById(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAssignmentByIdQuery(assignmentId),
            cancellationToken);

        return Ok(result);
    }
    // POST: /api/v1/courses/{courseId}/assignments
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