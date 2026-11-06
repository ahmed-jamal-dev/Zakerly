using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public sealed record GetAllLessonsQuery(
    Guid CourseId,
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetAllLessonsResponse>>;