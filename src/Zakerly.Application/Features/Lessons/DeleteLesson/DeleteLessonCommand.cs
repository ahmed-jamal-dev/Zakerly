using MediatR;

namespace Zakerly.Application.Features.Lessons.DeleteLesson;

public sealed record DeleteLessonCommand(
    Guid LessonId)
    : IRequest;