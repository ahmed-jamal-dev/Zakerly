namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public sealed record GetAllLessonsResponse(
    Guid LessonId,
    string Title,
    string Content);