using FluentValidation;

namespace Zakerly.Application.Features.Lessons.MarkLessonAsCompleted;

public class MarkLessonAsCompletedCommandValidator
    : AbstractValidator<MarkLessonAsCompletedCommand>
{
    public MarkLessonAsCompletedCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty();
    }
}