using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface IResourceRepository
{
    Task AddAsync(
        Resource resource,
        CancellationToken cancellationToken);

    Task<List<Resource>> GetAllByLessonIdAsync(
        Guid lessonId,
        CancellationToken cancellationToken);

    Task<Resource?> GetByIdAsync(
        Guid resourceId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Resource resource,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Resource resource,
        CancellationToken cancellationToken);
}