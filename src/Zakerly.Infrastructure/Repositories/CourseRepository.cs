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
            .Include(c => c.Instructor)
            .ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(
                c => 
                c.Id == id,
                cancellationToken);
    }
}