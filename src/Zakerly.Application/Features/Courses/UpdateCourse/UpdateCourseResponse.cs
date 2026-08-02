namespace Zakerly.Application.Features.Courses.UpdateCourse;

public sealed record UpdateCourseResponse(
    Guid CourseId,
    string Title,
    string Description,
    bool IsPublished
);