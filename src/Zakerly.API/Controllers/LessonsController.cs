using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.API.Contracts.Lessons;
using Zakerly.Application.Features.Lessons.CreateLesson;
using Zakerly.Application.Features.Lessons.DeleteLesson;
using Zakerly.Application.Features.Lessons.GetAllLessons;
using Zakerly.Application.Features.Lessons.GetLessonById;
using Zakerly.Application.Features.Lessons.UpdateLesson;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/lessons")]
public class LessonsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LessonsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET: api/v1/courses/{courseId}/lessons
    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllLessonsQuery(courseId),
            cancellationToken);
        return Ok(result);
    }
    // GET: api/v1/lessons/{lessonId}
    [HttpGet("/api/v1/lessons/{lessonId:guid}")]
    public async Task<IActionResult> GetById(
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLessonByIdQuery(lessonId),
            cancellationToken);

        return Ok(result);
    }
    // POST: api/v1/courses/{courseId}/lessons
    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid courseId,
        [FromBody] CreateLessonRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLessonCommand(
            courseId,
            request.Title,
            request.Content);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/lessons/{response.LessonId}",
            response);
    }
    // PUT: api/v1/lessons/{lessonId}
    [Authorize(Roles = "Instructor")]
    [HttpPut("/api/v1/lessons/{lessonId:guid}")]
    public async Task<IActionResult> Update(
        Guid lessonId,
        [FromBody] UpdateLessonRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLessonCommand(
            lessonId,
            request.Title,
            request.Content);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(response);
    }
    // DELETE: api/v1/lessons/{lessonId}
    [Authorize(Roles = "Instructor")]
    [HttpDelete("/api/v1/lessons/{lessonId:guid}")]
    public async Task<IActionResult> Delete(
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteLessonCommand(lessonId),
            cancellationToken);

        return NoContent();
    }
}