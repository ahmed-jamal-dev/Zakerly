using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly ZakerlyDbContext _context;

    public ResourceRepository(ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Resource resource,
        CancellationToken cancellationToken)
    {
        await _context.Resources.AddAsync(
            resource,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Resource>> GetAllByLessonIdAsync(
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        return await _context.Resources
            .Where(x => x.LessonId == lessonId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Resource?> GetByIdAsync(
        Guid resourceId,
        CancellationToken cancellationToken)
    {
        return await _context.Resources
            .FirstOrDefaultAsync(
                x => x.Id == resourceId,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Resource resource,
        CancellationToken cancellationToken)
    {
        _context.Resources.Update(resource);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Resource resource,
        CancellationToken cancellationToken)
    {
        _context.Resources.Remove(resource);

        await _context.SaveChangesAsync(cancellationToken);
    }
}