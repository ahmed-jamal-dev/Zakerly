namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public sealed record GetAllAssignmentsResponse(
    Guid AssignmentId,
    string Title,
    string Description);