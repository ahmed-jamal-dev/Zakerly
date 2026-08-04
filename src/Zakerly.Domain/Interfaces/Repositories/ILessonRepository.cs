using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ILessonRepository
{
    Task AddAsync(
        Lesson lesson,
        CancellationToken cancellationToken);
}