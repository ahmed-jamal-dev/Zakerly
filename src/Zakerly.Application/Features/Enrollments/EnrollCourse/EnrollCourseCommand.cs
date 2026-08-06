using MediatR;

namespace Zakerly.Application.Features.Enrollments.EnrollCourse;

public sealed record EnrollCourseCommand(
    Guid CourseId)
    : IRequest<EnrollCourseResponse>;