namespace Zakerly.API.Contracts.Submissions;

public sealed record GradeSubmissionRequest(
    decimal Grade,
    string? Feedback);