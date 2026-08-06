using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.SubmitAssignment;

public class SubmitAssignmentCommandHandler
    : IRequestHandler<SubmitAssignmentCommand, SubmitAssignmentResponse>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public SubmitAssignmentCommandHandler(
        ISubmissionRepository submissionRepository,
        IAssignmentRepository assignmentRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _assignmentRepository = assignmentRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<SubmitAssignmentResponse> Handle(
        SubmitAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.Role != UserRole.Student)
            throw new ForbiddenException(
                "Only students can submit assignments.");

        var assignment = await _assignmentRepository.GetByIdAsync(
            request.AssignmentId,
            cancellationToken);

        if (assignment is null)
            throw new NotFoundException(
                nameof(Assignment),
                request.AssignmentId);

        var enrolled = await _enrollmentRepository.ExistsAsync(
            _currentUserService.UserId,
            assignment.CourseId,
            cancellationToken);

        if (!enrolled)
            throw new ForbiddenException(
                "You are not enrolled in this course.");

        var alreadySubmitted =
            await _submissionRepository.ExistsAsync(
                request.AssignmentId,
                _currentUserService.UserId,
                cancellationToken);

        if (alreadySubmitted)
            throw new ConflictException(
                "You have already submitted this assignment.");

        var submission = new Submission(
            request.FilePath,
            request.AssignmentId,
            _currentUserService.UserId);

        await _submissionRepository.AddAsync(
            submission,
            cancellationToken);

        return new SubmitAssignmentResponse(
            submission.Id,
            submission.AssignmentId,
            submission.StudentId,
            submission.FilePath);
    }
}