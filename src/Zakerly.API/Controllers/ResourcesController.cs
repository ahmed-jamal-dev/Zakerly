using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Resources;
using Zakerly.Application.Features.Resources.CreateResource;
using Zakerly.Application.Features.Resources.DeleteResource;
using Zakerly.Application.Features.Resources.GetAllResources;
using Zakerly.Application.Features.Resources.GetResourceById;
using Zakerly.Application.Features.Resources.UpdateResource;

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
    // GET: api/v1/lessons/{lessonId}/resources
    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllResourcesQuery(lessonId),
            cancellationToken);

        return Ok(result);
    }
    // GET: api/v1/resources/{resourceId}
    [HttpGet("/api/v1/resources/{resourceId:guid}")]
    public async Task<IActionResult> GetById(
        Guid resourceId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetResourceByIdQuery(resourceId),
            cancellationToken);

        return Ok(result);
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
    
    // PUT: api/v1/resources/{resourceId}
    [Authorize(Roles = "Instructor")]
    [HttpPut("/api/v1/resources/{resourceId:guid}")]
    public async Task<IActionResult> Update(
        Guid resourceId,
        [FromBody] UpdateResourceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateResourceCommand(
            resourceId,
            request.Name,
            request.FilePath);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(response);
    }
    // DELETE: api/v1/resources/{resourceId}
    [Authorize(Roles = "Instructor")]
    [HttpDelete("/api/v1/resources/{resourceId:guid}")]
    public async Task<IActionResult> Delete(
        Guid resourceId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteResourceCommand(resourceId),
            cancellationToken);

        return NoContent();
    }
}