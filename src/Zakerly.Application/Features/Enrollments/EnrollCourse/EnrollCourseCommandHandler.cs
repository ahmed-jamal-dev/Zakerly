using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<EnrollCourseCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public EnrollCourseCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService,
        ILogger<EnrollCourseCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
        _logger = logger;
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

        if (course.IsArchived)
            throw new ConflictException(
                "This course is archived and cannot accept new enrollments.");

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

        // Create notification for the instructor
        await CreateNotificationForInstructor(enrollment, course, cancellationToken);

        return new EnrollCourseResponse(
            enrollment.Id,
            enrollment.CourseId,
            enrollment.StudentId);
    }

    private async Task CreateNotificationForInstructor(
        Enrollment enrollment,
        Course course,
        CancellationToken cancellationToken)
    {
        // Get the student's display name
        var student = await _userRepository.GetByIdAsync(enrollment.StudentId, cancellationToken);
        var studentName = student?.FullName ?? "A student";

        var notification = new Notification(
            recipientUserId: course.InstructorId,
            type: NotificationType.StudentEnrolled,
            title: "New Student Enrollment",
            message: $"{studentName} has enrolled in your course '{course.Title}'.",
            relatedEntityId: enrollment.Id,
            relatedEntityType: nameof(Enrollment));

        await _notificationRepository.AddAsync(notification, cancellationToken);

        // Send real-time notification via SignalR
        await _notificationService.SendNotificationAsync(notification, cancellationToken);
    }
}