using FluentValidation;

namespace Zakerly.Application.Features.Enrollments.CancelEnrollment;

public class CancelEnrollmentCommandValidator
    : AbstractValidator<CancelEnrollmentCommand>
{
    public CancelEnrollmentCommandValidator()
    {
        RuleFor(x => x.EnrollmentId)
            .NotEmpty();
    }
}