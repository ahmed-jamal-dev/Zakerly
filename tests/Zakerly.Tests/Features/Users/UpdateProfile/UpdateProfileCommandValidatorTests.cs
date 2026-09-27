using FluentValidation.TestHelper;
using Xunit;
using Zakerly.Application.Features.Users.UpdateProfile;

namespace Zakerly.Tests.Features.Users.UpdateProfile;

public class UpdateProfileCommandValidatorTests
{
    private readonly UpdateProfileCommandValidator _validator;

    public UpdateProfileCommandValidatorTests()
    {
        _validator = new UpdateProfileCommandValidator();
    }

    [Fact]
    public void Validate_WithValidFullNameAndEmail_HasNoValidationErrors()
    {
        // Arrange
        var command = new UpdateProfileCommand(
            FullName: "John Doe",
            Email: "john.doe@example.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyFullName_HasValidationErrorForFullName()
    {
        // Arrange
        var command = new UpdateProfileCommand(
            FullName: "",
            Email: "john.doe@example.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Validate_WithInvalidEmail_HasValidationErrorForEmail()
    {
        // Arrange
        var command = new UpdateProfileCommand(
            FullName: "John Doe",
            Email: "invalid-email");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmptyEmail_HasValidationErrorForEmail()
    {
        // Arrange
        var command = new UpdateProfileCommand(
            FullName: "John Doe",
            Email: "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
