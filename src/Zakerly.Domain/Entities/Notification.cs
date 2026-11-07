using Zakerly.Domain.Enums;

namespace Zakerly.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid RecipientUserId { get; private set; }

    public NotificationType Type { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public Guid? RelatedEntityId { get; private set; }

    public string? RelatedEntityType { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime? ReadAt { get; private set; }

    // Navigation Properties
    public User RecipientUser { get; private set; } = null!;

    private Notification()
    {
    }

    public Notification(
        Guid recipientUserId,
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null)
    {
        RecipientUserId = recipientUserId;
        Type = type;
        Title = title;
        Message = message;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}