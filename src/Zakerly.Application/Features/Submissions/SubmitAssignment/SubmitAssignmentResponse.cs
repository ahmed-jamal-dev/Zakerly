namespace Zakerly.Application.Features.Submissions.SubmitAssignment;

public sealed record SubmitAssignmentResponse(
    Guid SubmissionId,
    Guid AssignmentId,
    Guid StudentId,
    string FilePath);