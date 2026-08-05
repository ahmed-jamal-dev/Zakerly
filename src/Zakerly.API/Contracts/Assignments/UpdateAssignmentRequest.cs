namespace Zakerly.API.Contracts.Assignments;

public sealed record UpdateAssignmentRequest(
    string Title,
    string Description);