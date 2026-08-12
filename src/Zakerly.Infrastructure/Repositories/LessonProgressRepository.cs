using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly ZakerlyDbContext _context;

    public LessonProgressRepository(
        ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        LessonProgress progress,
        CancellationToken cancellationToken)
    {
        await _context.LessonProgresses.AddAsync(
            progress,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<LessonProgress?> GetByStudentAndLessonAsync(
        Guid studentId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        return await _context.LessonProgresses
            .FirstOrDefaultAsync(
                x => x.StudentId == studentId &&
                     x.LessonId == lessonId,
                cancellationToken);
    }

    public async Task<List<LessonProgress>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        return await _context.LessonProgresses
            .Include(x => x.Lesson)
            .Where(x => x.StudentId == studentId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LessonProgress>> GetByCourseIdAsync(
        Guid studentId,
        Guid courseId,
        CancellationToken cancellationToken)
    {
        return await _context.LessonProgresses
            .Include(x => x.Lesson)
            .Where(x =>
                x.StudentId == studentId &&
                x.Lesson.CourseId == courseId)
            .OrderBy(x => x.Lesson.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}