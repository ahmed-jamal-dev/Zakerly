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
            .Include(x => x.Assignment)
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Submission?> GetByIdAsync(
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        return await _context.Submissions
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
        return await _context.Submissions.AnyAsync(
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
    
}