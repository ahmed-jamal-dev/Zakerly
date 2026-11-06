using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface IEnrollmentRepository
{
    Task AddAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        Guid studentId,
        Guid courseId,
        CancellationToken cancellationToken);

    Task<List<Enrollment>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken);

    Task<List<Enrollment>> GetByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken);

    Task<Enrollment?> GetByIdAsync(
        Guid enrollmentId,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken);

    Task<long> CountByStudentIdAsync(Guid studentId, CancellationToken cancellationToken);
    Task<List<Enrollment>> GetByStudentIdPagedAsync(Guid studentId, long skip, int take, CancellationToken cancellationToken);

    Task<long> CountByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);
    Task<List<Enrollment>> GetByCourseIdPagedAsync(Guid courseId, long skip, int take, CancellationToken cancellationToken);
}