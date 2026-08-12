using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ILessonProgressRepository
{
    Task AddAsync(
        LessonProgress progress,
        CancellationToken cancellationToken);

    Task<LessonProgress?> GetByStudentAndLessonAsync(
        Guid studentId,
        Guid lessonId,
        CancellationToken cancellationToken);

    Task<List<LessonProgress>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken);

    Task<List<LessonProgress>> GetByCourseIdAsync(
        Guid studentId,
        Guid courseId,
        CancellationToken cancellationToken);
}