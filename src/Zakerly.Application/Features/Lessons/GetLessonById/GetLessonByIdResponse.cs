namespace Zakerly.Application.Features.Lessons.GetLessonById;

public sealed record GetLessonByIdResponse(
    Guid LessonId,
    string Title,
    string Content,
    Guid CourseId);