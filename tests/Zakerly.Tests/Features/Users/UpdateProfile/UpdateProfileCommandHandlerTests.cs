using FluentAssertions;
using MediatR;
using Moq;
using Zakerly.Application.Features.Users.UpdateProfile;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Application.Common.Interfaces;

namespace Zakerly.Tests.Features.Users.UpdateProfile;

public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        
        _handler = new UpdateProfileCommandHandler(
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_UpdatesUserProfile()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User(
            fullName: "Old Name",
            email: "old@example.com",
            passwordHash: "some-hash",
            role: UserRole.Student);
        
        // Set up the user with the correct ID (using reflection to set the private Id field)
        var userType = typeof(User);
        var idField = userType.GetProperty("Id");
        idField!.SetValue(existingUser, userId);
        
        var command = new UpdateProfileCommand(
            FullName: "New Name",
            Email: "new@example.com");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync("new@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.FullName.Should().Be("New Name");
        result.Email.Should().Be("new@example.com");
        
        _userRepositoryMock.Verify(x => x.UpdateAsync(
            It.Is<User>(u => 
                u.FullName == "New Name" && 
                u.Email == "new@example.com" &&
                u.Id == userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyFullName_ThrowsValidationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProfileCommand(
            FullName: "",
            Email: "valid@example.com");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == "FullName"));
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProfileCommand(
            FullName: "Valid Name",
            Email: "invalid-email");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == "Email"));
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User(
            fullName: "Current User",
            email: "current@example.com",
            passwordHash: "some-hash",
            role: UserRole.Student);
        
        // Set up the user with the correct ID
        var userType = typeof(User);
        var idField = userType.GetProperty("Id");
        idField!.SetValue(existingUser, userId);
        
        var command = new UpdateProfileCommand(
            FullName: "Updated Name",
            Email: "taken@example.com");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync("taken@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // Email already exists

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already in use*");
    }

    [Fact]
    public async Task Handle_WithNonExistingUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProfileCommand(
            FullName: "Valid Name",
            Email: "valid@example.com");

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null); // User not found

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {userId} not found");
    }
}
