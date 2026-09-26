using FluentAssertions;
using MediatR;
using Moq;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Features.Assignments.CreateAssignment;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Tests.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandHandlerTests
{
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateAssignmentCommandHandler _handler;

    public CreateAssignmentCommandHandlerTests()
    {
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        
        _handler = new CreateAssignmentCommandHandler(
            _assignmentRepositoryMock.Object,
            _courseRepositoryMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAssignmentResponse_When_CommandIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var title = "Test Assignment";
        var description = "Test Description";
        
        var command = new CreateAssignmentCommand(courseId, title, description);
        
        var course = new Zakerly.Domain.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            InstructorId = userId
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);
            
        _currentUserServiceMock.SetupGet(x => x.UserId)
            .Returns(userId);
            
        var assignment = new Zakerly.Domain.Entities.Assignment
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            CourseId = courseId
        };
        
        _assignmentRepositoryMock.Setup(x => x.AddAsync(
                It.IsAny<Zakerly.Domain.Entities.Assignment>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(assignment.Id);
        result.Title.Should().Be(assignment.Title);
        result.Description.Should().Be(assignment.Description);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_CourseDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var title = "Test Assignment";
        var description = "Test Description";
        var command = new CreateAssignmentCommand(courseId, title, description);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Zakerly.Domain.Entities.Course)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(ex => ex.EntityName == "Course" && ex.Key.Equals(courseId));
    }

    [Fact]
    public async Task Handle_Should_ThrowForbiddenException_When_UserIsNotInstructor()
    {
        // Arrange
        var userId = Guid.NewGuid(); // Current user
        var differentUserId = Guid.NewGuid(); // Course instructor
        var courseId = Guid.NewGuid();
        var title = "Test Assignment";
        var description = "Test Description";
        var command = new CreateAssignmentCommand(courseId, title, description);
        
        var course = new Zakerly.Domain.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            InstructorId = differentUserId // Different from current user
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);
            
        _currentUserServiceMock.SetupGet(x => x.UserId)
            .Returns(userId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("You are not allowed to add assignments to this course.");
    }
}
