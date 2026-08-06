using FluentValidation;

namespace Zakerly.Application.Features.Enrollments.EnrollCourse;

public class EnrollCourseCommandValidator
    : AbstractValidator<EnrollCourseCommand>
{
    public EnrollCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();
    }
}