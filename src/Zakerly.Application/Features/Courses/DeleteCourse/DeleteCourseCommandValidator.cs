using FluentValidation;

namespace Zakerly.Application.Features.Courses.DeleteCourse;

public class DeleteCourseCommandValidator
    : AbstractValidator<DeleteCourseCommand>
{
    public DeleteCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();
    }
}