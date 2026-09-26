using FluentAssertions;
using FluentValidation.TestHelper;
using Zakerly.Application.Features.Authentication.Login;

namespace Zakerly.Tests.Features.Authentication.Login;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public void Validate_Should_HaveNoErrors_When_CommandIsValid()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password123");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_HaveError_When_EmailIsEmpty()
    {
        // Arrange
        var command = new LoginCommand(string.Empty, "password123");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_Should_HaveError_When_EmailIsInvalidFormat()
    {
        // Arrange
        var command = new LoginCommand("invalid-email", "password123");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordIsEmpty()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
