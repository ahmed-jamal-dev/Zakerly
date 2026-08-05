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

}