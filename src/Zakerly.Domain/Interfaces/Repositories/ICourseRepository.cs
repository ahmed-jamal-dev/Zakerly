using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ICourseRepository
{
    Task AddAsync(Course course, CancellationToken cancellationToken);
    Task<List<Course>> GetAllAsync(CancellationToken cancellationToken);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Course course, CancellationToken cancellationToken);
    Task DeleteAsync(Course course, CancellationToken cancellationToken);
}