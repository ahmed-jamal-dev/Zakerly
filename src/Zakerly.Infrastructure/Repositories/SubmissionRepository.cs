using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly ZakerlyDbContext _context;

    public SubmissionRepository(
        ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Submission submission,
        CancellationToken cancellationToken)
    {
        await _context.Submissions.AddAsync(
            submission,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<List<Submission>> GetByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Assignment)
            .Where(x => x.AssignmentId == assignmentId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Submission>> GetByStudentIdAsync(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Include(x => x.Assignment)
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> CountByStudentIdAsync(Guid studentId, CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Where(s => s.StudentId == studentId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Submission>> GetByStudentIdPagedAsync(Guid studentId, long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Submissions
            .AsNoTracking()
            .Include(s => s.Assignment)
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.CreatedAt)
            .ThenByDescending(s => s.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> CountByAssignmentIdAsync(Guid assignmentId, CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Where(s => s.AssignmentId == assignmentId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<Submission>> GetByAssignmentIdPagedAsync(Guid assignmentId, long skip, int take, CancellationToken cancellationToken)
    {
        // Handle skip values that exceed int.MaxValue or are negative by returning empty list
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Submissions
            .AsNoTracking()
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .Where(s => s.AssignmentId == assignmentId)
            .OrderBy(s => s.CreatedAt)
            .ThenBy(s => s.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Submission?> GetByIdAsync(
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Assignment)
            .FirstOrDefaultAsync(
                x => x.Id == submissionId,
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid assignmentId,
        Guid studentId,
        CancellationToken cancellationToken)
    {
        return await _context.Submissions
            .AsNoTracking()
            .AnyAsync(
                x => x.AssignmentId == assignmentId &&
                     x.StudentId == studentId,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Submission submission,
        CancellationToken cancellationToken)
    {
        _context.Submissions.Update(submission);

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(
        Submission submission,
        CancellationToken cancellationToken)
    {
        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync(cancellationToken);
    }
}