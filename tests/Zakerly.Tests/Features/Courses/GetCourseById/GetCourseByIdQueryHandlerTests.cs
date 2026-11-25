using FluentAssertions;
using MediatR;
using Moq;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Features.Courses.GetCourseById;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Application.Common.Interfaces;

namespace Zakerly.Tests.Features.Courses.GetCourseById;

public class GetCourseByIdQueryHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly GetCourseByIdQueryHandler _handler;

    public GetCourseByIdQueryHandlerTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        _handler = new GetCourseByIdQueryHandler(
            _courseRepositoryMock.Object,
            _enrollmentRepositoryMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnCourseResponse_When_CourseExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var query = new GetCourseByIdQuery(courseId);
        var userId = Guid.NewGuid();

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

        _enrollmentRepositoryMock.Setup(x => x.ExistsAsync(userId, courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CourseId.Should().Be(course.Id);
        result.Title.Should().Be(course.Title);
        result.Description.Should().Be(course.Description);
        result.IsPublished.Should().Be(course.IsPublished);
        result.InstructorName.Should().Be(instructor.FullName);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_CourseDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var query = new GetCourseByIdQuery(courseId);

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Course?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(ex => ex.Message.Contains("Course") && ex.Message.Contains(courseId.ToString()));
    }
}