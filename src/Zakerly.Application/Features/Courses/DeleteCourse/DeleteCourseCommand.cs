using MediatR;

namespace Zakerly.Application.Features.Courses.DeleteCourse;

public sealed record DeleteCourseCommand(
    Guid CourseId
) : IRequest;