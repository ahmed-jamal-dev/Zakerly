namespace Zakerly.Application.Features.Courses.CreateCourse;

public record CreateCourseResponse(
    Guid CourseId,
    string Title,
    bool IsPublished
);