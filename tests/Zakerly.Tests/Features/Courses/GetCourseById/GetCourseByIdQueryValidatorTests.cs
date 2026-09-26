using FluentAssertions;
using FluentValidation.TestHelper;
using Zakerly.Application.Features.Courses.GetCourseById;

namespace Zakerly.Tests.Features.Courses.GetCourseById;

public class GetCourseByIdQueryValidatorTests
{
    private readonly GetCourseByIdQueryValidator _validator;

    public GetCourseByIdQueryValidatorTests()
    {
        _validator = new GetCourseByIdQueryValidator();
    }

    [Fact]
    public void Validate_Should_HaveNoErrors_When_QueryIsValid()
    {
        // Arrange
        var query = new GetCourseByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_HaveError_When_CourseIdIsEmpty()
    {
        // Arrange
        var query = new GetCourseByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }
}
