namespace Zakerly.Application.Features.Submissions.GetAssignmentSubmissions;

public sealed record GetAssignmentSubmissionsResponse(
    Guid SubmissionId,
    Guid StudentId,
    string StudentName,
    string StudentEmail,
    string FilePath,
    decimal? Grade,
    string? Feedback,
    DateTime SubmittedAt);