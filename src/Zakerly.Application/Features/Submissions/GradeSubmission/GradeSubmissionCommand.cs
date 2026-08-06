using MediatR;

namespace Zakerly.Application.Features.Submissions.GradeSubmission;

public sealed record GradeSubmissionCommand(
    Guid SubmissionId,
    decimal Grade,
    string? Feedback)
    : IRequest<GradeSubmissionResponse>;