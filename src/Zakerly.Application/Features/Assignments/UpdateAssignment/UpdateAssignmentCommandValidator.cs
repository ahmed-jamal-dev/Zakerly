using FluentValidation;

namespace Zakerly.Application.Features.Assignments.UpdateAssignment;

public class UpdateAssignmentCommandValidator
    : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}