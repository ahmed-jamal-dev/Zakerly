using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.CancelEnrollment;

public class CancelEnrollmentCommandHandler
    : IRequestHandler<CancelEnrollmentCommand>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public CancelEnrollmentCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        CancelEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(
            request.EnrollmentId,
            cancellationToken);

        if (enrollment is null)
            throw new NotFoundException(
                nameof(Enrollment),
                request.EnrollmentId);

        if (_currentUserService.Role != UserRole.Student)
            throw new ForbiddenException(
                "Only students can cancel enrollments.");

        if (enrollment.StudentId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to cancel this enrollment.");

        await _enrollmentRepository.DeleteAsync(
            enrollment,
            cancellationToken);
    }
}