using FluentAssertions;
using MediatR;
using Moq;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Features.Courses.GetCourseById;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Tests.Features.Courses.GetCourseById;

public class GetCourseByIdQueryHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly GetCourseByIdQueryHandler _handler;

    public GetCourseByIdQueryHandlerTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _handler = new GetCourseByIdQueryHandler(_courseRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnCourseResponse_When_CourseExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var query = new GetCourseByIdQuery(courseId);
        
        var course = new Zakerly.Domain.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            IsPublished = true,
            Instructor = new Zakerly.Domain.Entities.User
            {
                FullName = "Test Instructor"
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(course.Id);
        result.Title.Should().Be(course.Title);
        result.Description.Should().Be(course.Description);
        result.IsPublished.Should().Be(course.IsPublished);
        result.InstructorName.Should().Be(course.Instructor.FullName);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_CourseDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var query = new GetCourseByIdQuery(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Zakerly.Domain.Entities.Course)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(ex => ex.EntityName == "Course" && ex.Key.Equals(courseId));
    }
}
