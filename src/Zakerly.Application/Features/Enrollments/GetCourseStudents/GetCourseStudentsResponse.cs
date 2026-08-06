namespace Zakerly.Application.Features.Enrollments.GetCourseStudents;

public sealed record GetCourseStudentsResponse(
    Guid StudentId,
    string FullName,
    string Email,
    DateTime EnrolledAt);