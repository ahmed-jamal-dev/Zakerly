namespace Zakerly.Application.Features.Lessons.UpdateLesson;

public sealed record UpdateLessonResponse(
    Guid LessonId,
    string Title,
    string Content);