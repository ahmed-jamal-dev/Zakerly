using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Resources.GetAllResources;

public sealed record GetAllResourcesQuery(
    Guid LessonId,
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetAllResourcesResponse>>;