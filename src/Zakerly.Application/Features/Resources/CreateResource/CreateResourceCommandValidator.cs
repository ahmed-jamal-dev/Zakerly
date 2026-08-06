using FluentValidation;

namespace Zakerly.Application.Features.Resources.CreateResource;

public class CreateResourceCommandValidator
    : AbstractValidator<CreateResourceCommand>
{
    public CreateResourceCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.FilePath)
            .NotEmpty();
    }
}