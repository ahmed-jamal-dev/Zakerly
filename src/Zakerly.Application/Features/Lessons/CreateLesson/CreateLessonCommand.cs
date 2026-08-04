using MediatR;

namespace Zakerly.Application.Features.Lessons.CreateLesson;

public sealed record CreateLessonCommand(
    Guid CourseId,
    string Title,
    string Content)
    : IRequest<CreateLessonResponse>;