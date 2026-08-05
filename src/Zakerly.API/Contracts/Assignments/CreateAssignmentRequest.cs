namespace Zakerly.API.Contracts.Assignments;

public sealed record CreateAssignmentRequest(
    string Title,
    string Description);