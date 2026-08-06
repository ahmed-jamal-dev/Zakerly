using FluentValidation;

namespace Zakerly.Application.Features.Submissions.GradeSubmission;

public class GradeSubmissionCommandValidator
    : AbstractValidator<GradeSubmissionCommand>
{
    public GradeSubmissionCommandValidator()
    {
        RuleFor(x => x.SubmissionId)
            .NotEmpty();

        RuleFor(x => x.Grade)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.Feedback)
            .MaximumLength(1000);
    }
}