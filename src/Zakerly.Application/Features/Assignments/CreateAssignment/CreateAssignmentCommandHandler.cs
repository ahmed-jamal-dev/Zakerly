using MediatR;
using Microsoft.Extensions.Logging;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandHandler
    : IRequestHandler<CreateAssignmentCommand, CreateAssignmentResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CreateAssignmentCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService,
        ILogger<CreateAssignmentCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<CreateAssignmentResponse> Handle(
        CreateAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                request.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to add assignments to this course.");

        var assignment = new Assignment(
            request.Title,
            request.Description,
            request.CourseId,
            request.DueDate);

        await _assignmentRepository.AddAsync(
            assignment,
            cancellationToken);

        // Create notifications for enrolled students
        await CreateNotificationsForEnrolledStudents(assignment, course, cancellationToken);

        return new CreateAssignmentResponse(
            assignment.Id,
            assignment.Title,
            assignment.Description,
            assignment.DueDate);
    }

    private async Task CreateNotificationsForEnrolledStudents(
        Assignment assignment,
        Course course,
        CancellationToken cancellationToken)
    {
        var enrolledStudents = await _enrollmentRepository.GetStudentIdsByCourseIdAsync(
            course.Id,
            cancellationToken);

        foreach (var studentId in enrolledStudents)
        {
            var notification = new Notification(
                recipientUserId: studentId,
                type: NotificationType.AssignmentCreated,
                title: "New Assignment",
                message: $"A new assignment '{assignment.Title}' has been created in course '{course.Title}'.",
                relatedEntityId: assignment.Id,
                relatedEntityType: nameof(Assignment));

            await _notificationRepository.AddAsync(notification, cancellationToken);

            // Send real-time notification via SignalR
            await _notificationService.SendNotificationAsync(notification, cancellationToken);
        }
    }
}