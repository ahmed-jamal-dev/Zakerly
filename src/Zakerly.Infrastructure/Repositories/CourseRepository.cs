using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ZakerlyDbContext _context;

    public CourseRepository(ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken)
    {
        await _context.Courses.AddAsync(course, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Course>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(
                c =>
                c.Id == id,
                cancellationToken);
    }

    public async Task<long> CountPublishedNonArchivedAsync(CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.IsPublished && !c.IsArchived)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Course>> GetPublishedNonArchivedPagedAsync(long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .Where(c => c.IsPublished && !c.IsArchived)
            .OrderBy(c => c.CreatedAt)
            .ThenBy(c => c.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Course course,
        CancellationToken cancellationToken)
    {
        _context.Courses.Update(course);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Course course, CancellationToken cancellationToken)
    {
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);
    }
}