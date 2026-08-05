using MediatR;

namespace Zakerly.Application.Features.Assignments.UpdateAssignment;

public sealed record UpdateAssignmentCommand(
    Guid AssignmentId,
    string Title,
    string Description)
    : IRequest<UpdateAssignmentResponse>;