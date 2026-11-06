using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ZakerlyDbContext _context;

    public EnrollmentRepository(
        ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken)
    {
        await _context.Enrollments.AddAsync(
            enrollment,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid studentId,
        Guid courseId,
        CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .AnyAsync(
                x => x.StudentId == studentId &&
                     x.CourseId == courseId,
                cancellationToken);
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Include(x => x.Course)
            .Where(x => x.StudentId == studentId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Enrollment>> GetByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetByIdAsync(
        Guid enrollmentId,
        CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == enrollmentId,
                cancellationToken);
    }

    public async Task<long> CountByStudentIdAsync(Guid studentId, CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Enrollment>> GetByStudentIdPagedAsync(Guid studentId, long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId)
            .OrderBy(e => e.CreatedAt)
            .ThenBy(e => e.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> CountByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Enrollment>> GetByCourseIdPagedAsync(Guid courseId, long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Where(e => e.CourseId == courseId)
            .OrderBy(e => e.CreatedAt)
            .ThenBy(e => e.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken)
    {
        _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync(cancellationToken);
    }
}