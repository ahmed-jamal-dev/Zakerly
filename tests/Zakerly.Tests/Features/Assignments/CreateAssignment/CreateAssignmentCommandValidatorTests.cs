using FluentAssertions;
using FluentValidation.TestHelper;
using Zakerly.Application.Features.Assignments.CreateAssignment;

namespace Zakerly.Tests.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandValidatorTests
{
    private readonly CreateAssignmentCommandValidator _validator;

    public CreateAssignmentCommandValidatorTests()
    {
        _validator = new CreateAssignmentCommandValidator();
    }

    [Fact]
    public void Validate_Should_HaveNoErrors_When_CommandIsValid()
    {
        // Arrange
        var command = new CreateAssignmentCommand(
            Guid.NewGuid(),
            "Valid Title",
            "Valid Description");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_HaveError_When_CourseIdIsEmpty()
    {
        // Arrange
        var command = new CreateAssignmentCommand(
            Guid.Empty,
            "Valid Title",
            "Valid Description");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    [Fact]
    public void Validate_Should_HaveError_When_TitleIsEmpty()
    {
        // Arrange
        var command = new CreateAssignmentCommand(
            Guid.NewGuid(),
            string.Empty,
            "Valid Description");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_Should_HaveError_When_TitleExceedsMaxLength()
    {
        // Arrange
        var command = new CreateAssignmentCommand(
            Guid.NewGuid(),
            new string('A', 201), // 201 characters > 200 max
            "Valid Description");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_Should_HaveError_When_DescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateAssignmentCommand(
            Guid.NewGuid(),
            "Valid Title",
            string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}
