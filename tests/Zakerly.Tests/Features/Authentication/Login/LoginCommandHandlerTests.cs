using FluentAssertions;
using MediatR;
using Moq;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Features.Authentication.Login;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Domain.Interfaces.Security;

namespace Zakerly.Tests.Features.Authentication.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtProviderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnLoginResponse_When_CredentialsAreValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var command = new LoginCommand(email, password);
        
        var user = new Zakerly.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = "Test User",
            PasswordHash = "hashed_password"
        };
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
            
        _passwordHasherMock.Setup(x => x.Verify(password, user.PasswordHash))
            .Returns(true);
            
        var expectedToken = "valid_jwt_token";
        _jwtProviderMock.Setup(x => x.Generate(user))
            .Returns(expectedToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.FullName.Should().Be(user.FullName);
        result.Email.Should().Be(user.Email);
        result.Token.Should().Be(expectedToken);
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidCredentialsException_When_UserDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "password123";
        var command = new LoginCommand(email, password);
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Zakerly.Domain.Entities.User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidCredentialsException_When_PasswordIsIncorrect()
    {
        // Arrange
        var email = "test@example.com";
        var password = "wrong_password";
        var command = new LoginCommand(email, password);
        
        var user = new Zakerly.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = "Test User",
            PasswordHash = "hashed_password"
        };
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
            
        _passwordHasherMock.Setup(x => x.Verify(password, user.PasswordHash))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
