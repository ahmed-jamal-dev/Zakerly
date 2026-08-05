using MediatR;

namespace Zakerly.Application.Features.Assignments.CreateAssignment;

public sealed record CreateAssignmentCommand(
    Guid CourseId,
    string Title,
    string Description)
    : IRequest<CreateAssignmentResponse>;