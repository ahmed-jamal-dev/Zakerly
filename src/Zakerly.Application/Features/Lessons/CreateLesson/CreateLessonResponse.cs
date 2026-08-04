namespace Zakerly.Application.Features.Lessons.CreateLesson;

public sealed record CreateLessonResponse(
    Guid LessonId,
    string Title,
    string Content,
    Guid CourseId);