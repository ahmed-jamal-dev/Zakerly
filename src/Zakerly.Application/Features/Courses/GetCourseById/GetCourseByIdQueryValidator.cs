using FluentValidation;

namespace Zakerly.Application.Features.Courses.GetCourseById;

public class GetCourseByIdQueryValidator
    : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("Course ID is required.");
    }
}