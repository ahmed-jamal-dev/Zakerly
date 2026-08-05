using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly ZakerlyDbContext _context;

    public LessonRepository(
        ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Lesson lesson,
        CancellationToken cancellationToken)
    {
        await _context.Lessons.AddAsync(
            lesson,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    public async Task<List<Lesson>> GetAllByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        return await _context.Lessons
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}