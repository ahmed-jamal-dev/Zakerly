using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Enrollments.GetCourseStudents;

public sealed record GetCourseStudentsQuery(
    Guid CourseId,
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetCourseStudentsResponse>>;