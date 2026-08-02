namespace Zakerly.Application.Features.Courses.GetCourseById;

public sealed record GetCourseByIdResponse(
    Guid CourseId,
    string Title,
    string Description,
    bool IsPublished,
    string InstructorName
);