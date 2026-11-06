namespace Zakerly.Application.Features.Courses.UnarchiveCourse;

public sealed record UnarchiveCourseResponse(
    Guid CourseId,
    string Title,
    string Description,
    bool IsPublished,
    bool IsArchived);