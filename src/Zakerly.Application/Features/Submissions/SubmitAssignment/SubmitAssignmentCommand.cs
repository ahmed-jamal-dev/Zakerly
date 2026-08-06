using MediatR;

namespace Zakerly.Application.Features.Submissions.SubmitAssignment;

public sealed record SubmitAssignmentCommand(
    Guid AssignmentId,
    string FilePath)
    : IRequest<SubmitAssignmentResponse>;