using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ZakerlyDbContext _context;

    public AssignmentRepository(
        ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken)
    {
        await _context.Assignments.AddAsync(
            assignment,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<List<Assignment>> GetAllByCourseIdAsync(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        return await _context.Assignments
            .AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Assignment?> GetByIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        return await _context.Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == assignmentId,
                cancellationToken);
    }

    public async Task<Assignment?> GetByIdWithCourseAsync(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        return await _context.Assignments
            .AsNoTracking()
            .Include(a => a.Course)
            .FirstOrDefaultAsync(
                x => x.Id == assignmentId,
                cancellationToken);
    }

    public async Task<long> CountByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
    {
        return await _context.Assignments
            .AsNoTracking()
            .Where(a => a.CourseId == courseId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Assignment>> GetByCourseIdPagedAsync(Guid courseId, long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Assignments
            .AsNoTracking()
            .Where(a => a.CourseId == courseId)
            .OrderBy(a => a.CreatedAt)
            .ThenBy(a => a.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Assignment assignment,
        CancellationToken cancellationToken)
    {
        _context.Assignments.Update(assignment);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        Assignment assignment,
        CancellationToken cancellationToken)
    {
        _context.Assignments.Remove(assignment);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}