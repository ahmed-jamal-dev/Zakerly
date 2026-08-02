using MediatR;

namespace Zakerly.Application.Features.Courses.UpdateCourse;

public sealed record UpdateCourseCommand(
    Guid CourseId,
    string Title,
    string Description)
    : IRequest<UpdateCourseResponse>;