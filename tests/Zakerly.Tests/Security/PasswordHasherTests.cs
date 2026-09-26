using FluentAssertions;
using Zakerly.Infrastructure.Security;

namespace Zakerly.Tests.Security;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher;

    public PasswordHasherTests()
    {
        _hasher = new PasswordHasher();
    }

    [Fact]
    public void Hash_Should_ReturnHashedPassword_When_InputIsValid()
    {
        // Arrange
        var password = "my_secure_password";

        // Act
        var hashed = _hasher.Hash(password);

        // Assert
        hashed.Should().NotBeNullOrEmpty();
        hashed.Should().NotBe(password); // Should be different from original
        hashed.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Verify_Should_ReturnTrue_When_PasswordMatchesHash()
    {
        // Arrange
        var password = "my_secure_password";
        var hashed = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(password, hashed);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_Should_ReturnFalse_When_PasswordDoesNotMatchHash()
    {
        // Arrange
        var password = "my_secure_password";
        var wrongPassword = "wrong_password";
        var hashed = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(wrongPassword, hashed);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Verify_Should_ReturnFalse_When_InputsAreEmpty()
    {
        // Act
        var result = _hasher.Verify(string.Empty, string.Empty);

        // Assert
        result.Should().BeFalse();
    }
}
