namespace Zakerly.Application.Features.Submissions.GradeSubmission;

public sealed record GradeSubmissionResponse(
    Guid SubmissionId,
    decimal Grade,
    string? Feedback);