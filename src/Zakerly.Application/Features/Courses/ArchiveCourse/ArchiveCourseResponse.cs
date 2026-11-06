namespace Zakerly.Application.Features.Courses.ArchiveCourse;

public sealed record ArchiveCourseResponse(
    Guid CourseId,
    string Title,
    string Description,
    bool IsPublished,
    bool IsArchived);