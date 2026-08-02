using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface ICourseRepository
{
    Task AddAsync(Course course, CancellationToken cancellationToken);
    Task<List<Course>> GetAllAsync(CancellationToken cancellationToken);
}