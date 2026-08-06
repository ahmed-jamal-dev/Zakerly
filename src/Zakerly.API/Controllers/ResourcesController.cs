using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Resources;
using Zakerly.Application.Features.Resources.CreateResource;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/lessons/{lessonId:guid}/resources")]
public class ResourcesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourcesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/v1/lessons/{lessonId}/resources
    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid lessonId,
        [FromBody] CreateResourceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateResourceCommand(
            lessonId,
            request.Name,
            request.FilePath);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/resources/{response.ResourceId}",
            response);
    }
}