using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Features.Courses.CreateCourse;
using Zakerly.Application.Features.Courses.GetAllCourses;
using Zakerly.Application.Features.Courses.GetCourseById;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/v1/courses
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllCoursesQuery(),
            cancellationToken);

        return Ok(result);
    }

    // GET: api/v1/courses/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCourseByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    // POST: api/v1/courses
    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse(
        [FromBody] CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/courses/{response.CourseId}",
            response);
    }
}