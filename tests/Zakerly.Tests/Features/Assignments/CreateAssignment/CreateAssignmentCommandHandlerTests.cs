using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Features.Assignments.CreateAssignment;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Tests.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandHandlerTests
{
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly Mock<INotificationRepository> _notificationRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ILogger<CreateAssignmentCommandHandler>> _loggerMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateAssignmentCommandHandler _handler;

    public CreateAssignmentCommandHandlerTests()
    {
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _notificationServiceMock = new Mock<INotificationService>();
        _loggerMock = new Mock<ILogger<CreateAssignmentCommandHandler>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        _handler = new CreateAssignmentCommandHandler(
            _assignmentRepositoryMock.Object,
            _courseRepositoryMock.Object,
            _enrollmentRepositoryMock.Object,
            _notificationRepositoryMock.Object,
            _notificationServiceMock.Object,
            _loggerMock.Object,
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
        var dueDate = DateTime.UtcNow.AddDays(7);

        var command = new CreateAssignmentCommand(courseId, title, description, dueDate);

        var instructor = new User("Test Instructor", "instructor@example.com", "hash", UserRole.Instructor);
        var instructorType = typeof(User);
        var instructorIdField = instructorType.GetProperty("Id");
        instructorIdField!.SetValue(instructor, userId);

        var course = new Course("Test Course", "Test Description", userId);
        var courseType = typeof(Course);
        var courseIdField = courseType.GetProperty("Id");
        courseIdField!.SetValue(course, courseId);

        var isPublishedField = courseType.GetProperty("IsPublished");
        isPublishedField!.SetValue(course, true);

        var instructorField = courseType.GetProperty("Instructor");
        instructorField!.SetValue(course, instructor);

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _currentUserServiceMock.SetupGet(x => x.UserId)
            .Returns(userId);

        _enrollmentRepositoryMock.Setup(x => x.GetStudentIdsByCourseIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Set up AddAsync to assign an Id to the assignment (simulating EF Core behavior)
        _assignmentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()))
            .Callback<Assignment, CancellationToken>((a, _) =>
            {
                var assignmentType = typeof(Assignment);
                var idProperty = assignmentType.GetProperty("Id");
                idProperty!.SetValue(a, Guid.NewGuid());
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AssignmentId.Should().NotBeEmpty();
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
        result.DueDate.Should().Be(dueDate);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_CourseDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var title = "Test Assignment";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(7);
        var command = new CreateAssignmentCommand(courseId, title, description, dueDate);

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Course?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(ex => ex.Message.Contains("Course") && ex.Message.Contains(courseId.ToString()));
    }

    [Fact]
    public async Task Handle_Should_ThrowForbiddenException_When_UserIsNotInstructor()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var title = "Test Assignment";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(7);
        var command = new CreateAssignmentCommand(courseId, title, description, dueDate);

        var instructor = new User("Test Instructor", "instructor@example.com", "hash", UserRole.Instructor);
        var instructorType = typeof(User);
        var instructorIdField = instructorType.GetProperty("Id");
        instructorIdField!.SetValue(instructor, differentUserId);

        var course = new Course("Test Course", "Test Description", differentUserId);
        var courseType = typeof(Course);
        var courseIdField = courseType.GetProperty("Id");
        courseIdField!.SetValue(course, courseId);

        var isPublishedField = courseType.GetProperty("IsPublished");
        isPublishedField!.SetValue(course, true);

        var instructorField = courseType.GetProperty("Instructor");
        instructorField!.SetValue(course, instructor);

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