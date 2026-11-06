using MediatR;
using Zakerly.Application.Common.Pagination;

namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public sealed record GetAllAssignmentsQuery(
    Guid CourseId,
    PaginationParams? Pagination = null)
    : IRequest<PagedResponse<GetAllAssignmentsResponse>>;