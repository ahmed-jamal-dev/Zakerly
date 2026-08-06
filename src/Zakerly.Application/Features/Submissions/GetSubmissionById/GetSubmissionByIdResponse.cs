namespace Zakerly.Application.Features.Submissions.GetSubmissionById;

public sealed record GetSubmissionByIdResponse(
    Guid SubmissionId,
    Guid AssignmentId,
    Guid StudentId,
    string StudentName,
    string StudentEmail,
    string FilePath,
    decimal? Grade,
    string? Feedback,
    DateTime SubmittedAt);