using MediatR;

namespace Zakerly.Application.Features.Assignments.GetAssignmentById;

public sealed record GetAssignmentByIdQuery(
    Guid AssignmentId)
    : IRequest<GetAssignmentByIdResponse>;