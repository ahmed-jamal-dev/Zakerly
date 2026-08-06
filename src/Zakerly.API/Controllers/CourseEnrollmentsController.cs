using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Features.Enrollments.EnrollCourse;
using Zakerly.Application.Features.Enrollments.GetMyEnrollments;

namespace Zakerly.API.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/enrollments")]
public class CourseEnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourseEnrollmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // POST: api/v1/courses/{courseId}/enrollments
    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> Enroll(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var command = new EnrollCourseCommand(courseId);

        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/v1/enrollments/{response.EnrollmentId}",
            response);
    }
    
}