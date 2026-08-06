namespace Zakerly.Application.Features.Enrollments.EnrollCourse;

public sealed record EnrollCourseResponse(
    Guid EnrollmentId,
    Guid CourseId,
    Guid StudentId);