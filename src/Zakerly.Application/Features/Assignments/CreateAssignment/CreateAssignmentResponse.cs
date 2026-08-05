namespace Zakerly.Application.Features.Assignments.CreateAssignment;

public sealed record CreateAssignmentResponse(
    Guid AssignmentId,
    string Title,
    string Description);