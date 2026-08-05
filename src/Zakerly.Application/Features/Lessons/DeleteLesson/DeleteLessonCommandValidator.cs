using FluentValidation;

namespace Zakerly.Application.Features.Lessons.DeleteLesson;

public class DeleteLessonCommandValidator
    : AbstractValidator<DeleteLessonCommand>
{
    public DeleteLessonCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty();
    }
}