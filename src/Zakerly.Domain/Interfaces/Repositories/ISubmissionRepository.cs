using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ISubmissionRepository
{
    Task AddAsync(
        Submission submission,
        CancellationToken cancellationToken);

    Task<List<Submission>> GetByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken);

    Task<List<Submission>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken);

    Task<Submission?> GetByIdAsync(
        Guid submissionId,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        Guid assignmentId,
        Guid studentId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Submission submission,
        CancellationToken cancellationToken);
    Task DeleteAsync(
        Submission submission,
        CancellationToken cancellationToken);

    Task<long> CountByStudentIdAsync(Guid studentId, CancellationToken cancellationToken);
    Task<List<Submission>> GetByStudentIdPagedAsync(Guid studentId, long skip, int take, CancellationToken cancellationToken);

    Task<long> CountByAssignmentIdAsync(Guid assignmentId, CancellationToken cancellationToken);
    Task<List<Submission>> GetByAssignmentIdPagedAsync(Guid assignmentId, long skip, int take, CancellationToken cancellationToken);
}