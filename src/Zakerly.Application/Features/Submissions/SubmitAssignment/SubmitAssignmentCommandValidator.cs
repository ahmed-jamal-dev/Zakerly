using FluentValidation;

namespace Zakerly.Application.Features.Submissions.SubmitAssignment;

public class SubmitAssignmentCommandValidator
    : AbstractValidator<SubmitAssignmentCommand>
{
    public SubmitAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId)
            .NotEmpty();

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .MaximumLength(500);
    }
}