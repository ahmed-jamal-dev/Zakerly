namespace Zakerly.Application.Features.Submissions.GetMySubmissions;

public sealed record GetMySubmissionsResponse(
    Guid SubmissionId,
    Guid AssignmentId,
    string AssignmentTitle,
    string FilePath,
    decimal? Grade,
    string? Feedback,
    DateTime SubmittedAt);