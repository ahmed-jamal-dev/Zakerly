using FluentValidation;

namespace Zakerly.Application.Features.Submissions.DeleteSubmission;

public class DeleteSubmissionCommandValidator
    : AbstractValidator<DeleteSubmissionCommand>
{
    public DeleteSubmissionCommandValidator()
    {
        RuleFor(x => x.SubmissionId)
            .NotEmpty();
    }
}