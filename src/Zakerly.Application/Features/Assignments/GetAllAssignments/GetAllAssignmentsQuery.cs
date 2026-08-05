using MediatR;

namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public sealed record GetAllAssignmentsQuery(
    Guid CourseId)
    : IRequest<List<GetAllAssignmentsResponse>>;