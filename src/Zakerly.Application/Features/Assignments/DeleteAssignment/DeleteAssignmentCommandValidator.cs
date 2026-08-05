using FluentValidation;

namespace Zakerly.Application.Features.Assignments.DeleteAssignment;

public class DeleteAssignmentCommandValidator
    : AbstractValidator<DeleteAssignmentCommand>
{
    public DeleteAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId)
            .NotEmpty();
    }
}