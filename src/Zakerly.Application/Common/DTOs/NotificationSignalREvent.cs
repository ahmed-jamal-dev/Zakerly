using Zakerly.Domain.Enums;

namespace Zakerly.Application.Common.DTOs;

public record NotificationSignalREvent(
    Guid Id,
    NotificationType Type,
    string Title,
    string Message,
    Guid? RelatedEntityId,
    string? RelatedEntityType,
    DateTime CreatedAt,
    bool IsRead);