using MediatR;

namespace Zakerly.Application.Features.Lessons.UpdateLesson;

public sealed record UpdateLessonCommand(
    Guid LessonId,
    string Title,
    string Content)
    : IRequest<UpdateLessonResponse>;