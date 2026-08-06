using MediatR;

namespace Zakerly.Application.Features.Enrollments.GetCourseStudents;

public sealed record GetCourseStudentsQuery(
    Guid CourseId)
    : IRequest<List<GetCourseStudentsResponse>>;