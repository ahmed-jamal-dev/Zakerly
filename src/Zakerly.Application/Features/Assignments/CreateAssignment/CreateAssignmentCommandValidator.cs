using FluentValidation;

namespace Zakerly.Application.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandValidator
    : AbstractValidator<CreateAssignmentCommand>
{
    public CreateAssignmentCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}