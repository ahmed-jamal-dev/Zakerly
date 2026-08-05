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
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Assignment?> GetByIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        return await _context.Assignments
            .FirstOrDefaultAsync(
                x => x.Id == assignmentId,
                cancellationToken);
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