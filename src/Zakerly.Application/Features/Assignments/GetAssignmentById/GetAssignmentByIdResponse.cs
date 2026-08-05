namespace Zakerly.Application.Features.Assignments.GetAssignmentById;

public sealed record GetAssignmentByIdResponse(
    Guid AssignmentId,
    string Title,
    string Description,
    Guid CourseId);