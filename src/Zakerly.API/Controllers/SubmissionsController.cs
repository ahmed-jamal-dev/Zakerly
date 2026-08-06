using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Submissions;
using Zakerly.Application.Features.Submissions.GetMySubmissions;
using Zakerly.Application.Features.Submissions.SubmitAssignment;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/assignments/{assignmentId:guid}/submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubmissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/v1/assignments/{assignmentId}/submissions
    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> Submit(
        Guid assignmentId,
        [FromBody] SubmitAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SubmitAssignmentCommand(
            assignmentId,
            request.FilePath);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/submissions/{response.SubmissionId}",
            response);
    }
    // GET: api/v1/submissions/my
    [Authorize(Roles = "Student")]
    [HttpGet("/api/v1/submissions/my")]
    public async Task<IActionResult> GetMySubmissions(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMySubmissionsQuery(),
            cancellationToken);

        return Ok(result);
    }
    
}