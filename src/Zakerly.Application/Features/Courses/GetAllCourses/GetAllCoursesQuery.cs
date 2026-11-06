using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Courses.GetAllCourses;

public sealed record GetAllCoursesQuery(
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetAllCoursesResponse>>;