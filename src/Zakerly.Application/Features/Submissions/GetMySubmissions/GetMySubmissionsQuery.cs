using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Submissions.GetMySubmissions;

public sealed record GetMySubmissionsQuery(
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetMySubmissionsResponse>>;