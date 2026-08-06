using MediatR;

namespace Zakerly.Application.Features.Enrollments.CancelEnrollment;

public sealed record CancelEnrollmentCommand(
    Guid EnrollmentId)
    : IRequest;