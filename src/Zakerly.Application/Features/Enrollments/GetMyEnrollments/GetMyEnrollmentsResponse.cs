namespace Zakerly.Application.Features.Enrollments.GetMyEnrollments;

public sealed record GetMyEnrollmentsResponse(
    Guid EnrollmentId,
    Guid CourseId,
    string CourseTitle,
    bool IsPublished);