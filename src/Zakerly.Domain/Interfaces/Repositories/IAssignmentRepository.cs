using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface IAssignmentRepository
{
    Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken);

    Task<List<Assignment>> GetAllByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken);

    Task<Assignment?> GetByIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Assignment assignment,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Assignment assignment,
        CancellationToken cancellationToken);
}