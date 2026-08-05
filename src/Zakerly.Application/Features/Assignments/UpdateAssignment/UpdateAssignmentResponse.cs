namespace Zakerly.Application.Features.Assignments.UpdateAssignment;

public sealed record UpdateAssignmentResponse(
    Guid AssignmentId,
    string Title,
    string Description);