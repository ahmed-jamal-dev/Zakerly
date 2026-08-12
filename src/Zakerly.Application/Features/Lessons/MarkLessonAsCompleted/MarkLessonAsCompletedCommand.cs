using MediatR;

namespace Zakerly.Application.Features.Lessons.MarkLessonAsCompleted;

public sealed record MarkLessonAsCompletedCommand(
    Guid LessonId)
    : IRequest<MarkLessonAsCompletedResponse>;