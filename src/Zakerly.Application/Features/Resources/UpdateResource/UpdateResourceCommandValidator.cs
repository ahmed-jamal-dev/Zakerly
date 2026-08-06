using FluentValidation;

namespace Zakerly.Application.Features.Resources.UpdateResource;

public class UpdateResourceCommandValidator
    : AbstractValidator<UpdateResourceCommand>
{
    public UpdateResourceCommandValidator()
    {
        RuleFor(x => x.ResourceId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.FilePath)
            .NotEmpty();
    }
}