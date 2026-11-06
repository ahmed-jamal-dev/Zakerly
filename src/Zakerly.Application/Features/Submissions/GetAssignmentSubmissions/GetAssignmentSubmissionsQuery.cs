using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Submissions.GetAssignmentSubmissions;

public sealed record GetAssignmentSubmissionsQuery(
    Guid AssignmentId,
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetAssignmentSubmissionsResponse>>;