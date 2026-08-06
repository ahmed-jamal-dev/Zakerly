using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.EnrollCourse;

public class EnrollCourseCommandHandler
    : IRequestHandler<EnrollCourseCommand, EnrollCourseResponse>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public EnrollCourseCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<EnrollCourseResponse> Handle(
        EnrollCourseCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.Role != UserRole.Student)
            throw new ForbiddenException(
                "Only students can enroll in courses.");

        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                request.CourseId);

        if (!course.IsPublished)
            throw new ConflictException(
                "This course is not published yet.");

        var alreadyEnrolled =
            await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                request.CourseId,
                cancellationToken);

        if (alreadyEnrolled)
            throw new ConflictException(
                "You are already enrolled in this course.");

        var enrollment = new Enrollment(
            _currentUserService.UserId,
            request.CourseId);

        await _enrollmentRepository.AddAsync(
            enrollment,
            cancellationToken);

        return new EnrollCourseResponse(
            enrollment.Id,
            enrollment.CourseId,
            enrollment.StudentId);
    }
}