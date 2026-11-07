using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface INotificationRepository
{
    Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken);

    Task<Notification?> GetByIdAsync(
        Guid notificationId,
        CancellationToken cancellationToken);

    Task<List<Notification>> GetByRecipientIdPagedAsync(
        Guid recipientUserId,
        long skip,
        int take,
        CancellationToken cancellationToken);

    Task<long> CountByRecipientIdAsync(
        Guid recipientUserId,
        CancellationToken cancellationToken);

    Task<long> CountUnreadByRecipientIdAsync(
        Guid recipientUserId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Notification notification,
        CancellationToken cancellationToken);

    Task<int> MarkAllAsReadAsync(
        Guid recipientUserId,
        CancellationToken cancellationToken);
}