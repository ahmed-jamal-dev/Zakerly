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
}