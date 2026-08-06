using MediatR;

namespace Zakerly.Application.Features.Submissions.DeleteSubmission;

public sealed record DeleteSubmissionCommand(
    Guid SubmissionId)
    : IRequest;