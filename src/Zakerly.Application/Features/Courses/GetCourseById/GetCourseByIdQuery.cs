using MediatR;

namespace Zakerly.Application.Features.Courses.GetCourseById;

public sealed record GetCourseByIdQuery(
    Guid CourseId
) : IRequest<GetCourseByIdResponse>;