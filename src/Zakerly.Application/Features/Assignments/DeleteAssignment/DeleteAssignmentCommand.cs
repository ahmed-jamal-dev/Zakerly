using MediatR;

namespace Zakerly.Application.Features.Assignments.DeleteAssignment;

public sealed record DeleteAssignmentCommand(
    Guid AssignmentId)
    : IRequest;