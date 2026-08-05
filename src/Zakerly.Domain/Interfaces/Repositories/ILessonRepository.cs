using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ILessonRepository
{
    Task AddAsync(
        Lesson lesson,
        CancellationToken cancellationToken);

    Task<List<Lesson>> GetAllByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken
    );
    Task<Lesson?> GetByIdAsync(
        Guid lessonId,
        CancellationToken cancellationToken);
    Task UpdateAsync(
        Lesson lesson,
        CancellationToken cancellationToken);
    Task DeleteAsync(
        Lesson lesson,
        CancellationToken cancellationToken);

}