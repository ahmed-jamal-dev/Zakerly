using FluentValidation;

namespace Zakerly.Application.Features.Resources.DeleteResource;

public class DeleteResourceCommandValidator
    : AbstractValidator<DeleteResourceCommand>
{
    public DeleteResourceCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty();
    }
}