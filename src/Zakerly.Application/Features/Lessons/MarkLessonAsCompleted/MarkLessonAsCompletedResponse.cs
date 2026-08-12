namespace Zakerly.Application.Features.Lessons.MarkLessonAsCompleted;

public sealed record MarkLessonAsCompletedResponse(
    Guid LessonProgressId,
    Guid LessonId,
    bool IsCompleted,
    DateTime? CompletedAt);