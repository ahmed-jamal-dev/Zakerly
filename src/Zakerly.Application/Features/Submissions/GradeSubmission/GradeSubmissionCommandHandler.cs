using MediatR;
using Microsoft.Extensions.Logging;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GradeSubmission;

public class GradeSubmissionCommandHandler
    : IRequestHandler<GradeSubmissionCommand, GradeSubmissionResponse>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<GradeSubmissionCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public GradeSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        ICourseRepository courseRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService,
        ILogger<GradeSubmissionCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _courseRepository = courseRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<GradeSubmissionResponse> Handle(
        GradeSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var submission = await _submissionRepository.GetByIdAsync(
            request.SubmissionId,
            cancellationToken);

        if (submission is null)
            throw new NotFoundException(
                nameof(Submission),
                request.SubmissionId);

        var course = await _courseRepository.GetByIdAsync(
            submission.Assignment.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                submission.Assignment.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to grade this submission.");

        submission.GradeSubmission(
            request.Grade,
            request.Feedback);

        await _submissionRepository.UpdateAsync(
            submission,
            cancellationToken);

        // Create notification for the student
        await CreateNotificationForStudent(submission, cancellationToken);

        return new GradeSubmissionResponse(
            submission.Id,
            submission.Grade!.Value,
            submission.Feedback);
    }

    private async Task CreateNotificationForStudent(
        Submission submission,
        CancellationToken cancellationToken)
    {
        var notification = new Notification(
            recipientUserId: submission.StudentId,
            type: NotificationType.SubmissionGraded,
            title: "Assignment Graded",
            message: $"Your submission for assignment '{submission.Assignment.Title}' in course '{submission.Assignment.Course.Title}' has been graded.",
            relatedEntityId: submission.Id,
            relatedEntityType: nameof(Submission));

        await _notificationRepository.AddAsync(notification, cancellationToken);

        // Send real-time notification via SignalR
        await _notificationService.SendNotificationAsync(notification, cancellationToken);
    }
}