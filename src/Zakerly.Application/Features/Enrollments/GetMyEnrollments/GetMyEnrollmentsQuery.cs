using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Enrollments.GetMyEnrollments;

public sealed record GetMyEnrollmentsQuery(
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetMyEnrollmentsResponse>>;