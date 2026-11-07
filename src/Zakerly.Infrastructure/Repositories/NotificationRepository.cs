using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;

namespace Zakerly.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ZakerlyDbContext _context;

    public NotificationRepository(ZakerlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
    }

    public async Task<List<Notification>> GetByRecipientIdPagedAsync(Guid recipientUserId, long skip, int take, CancellationToken cancellationToken)
    {
        if (skip < 0 || skip > int.MaxValue)
        {
            return [];
        }

        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.RecipientUserId == recipientUserId)
            .OrderByDescending(n => n.CreatedAt)
            .ThenByDescending(n => n.Id)
            .Skip((int)skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> CountByRecipientIdAsync(Guid recipientUserId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.RecipientUserId == recipientUserId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<long> CountUnreadByRecipientIdAsync(Guid recipientUserId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.RecipientUserId == recipientUserId && !n.IsRead)
            .LongCountAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> MarkAllAsReadAsync(Guid recipientUserId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        return await _context.Notifications
            .Where(n => n.RecipientUserId == recipientUserId && !n.IsRead)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAt, now)
                .SetProperty(n => n.UpdatedAt, now), cancellationToken);
    }
}