namespace Zakerly.Application.Features.Courses.GetAllCourses;

public sealed record GetAllCoursesResponse(
    Guid CourseId,
    string Title,
    string Description,
    bool IsPublished,
    string InstructorName
);