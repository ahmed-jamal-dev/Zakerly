using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Infrastructure.Security;

namespace Zakerly.Tests.Security;

public class JwtProviderTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly JwtProvider _provider;

    public JwtProviderTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        
        // Setup configuration values
        _configurationMock.Setup(x => x["JwtSettings:Key"])
            .Returns("super_secret_key_that_is_long_enough_for_valid_key");
        _configurationMock.Setup(x => x["JwtSettings:Issuer"])
            .Returns("test_issuer");
        _configurationMock.Setup(x => x["JwtSettings:Audience"])
            .Returns("test_audience");
        _configurationMock.Setup(x => x["JwtSettings:ExpiryInMinutes"])
            .Returns("60");
        
        _provider = new JwtProvider(_configurationMock.Object);
    }

    [Fact]
    public void Generate_Should_ReturnValidToken_When_UserIsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = UserRole.Instructor
        };

        // Act
        var token = _provider.Generate(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
        token.Should().Contain("."); // JWT tokens have dots separating parts
    }
}
