using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.DeleteSubmission;

public class DeleteSubmissionCommandHandler
    : IRequestHandler<DeleteSubmissionCommand>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        IAssignmentRepository assignmentRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _assignmentRepository = assignmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var submission = await _submissionRepository.GetByIdAsync(
            request.SubmissionId,
            cancellationToken);

        if (submission is null)
            throw new NotFoundException(
                nameof(Submission),
                request.SubmissionId);

        if (_currentUserService.Role != UserRole.Student)
            throw new ForbiddenException(
                "Only students can delete submissions.");

        if (submission.StudentId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to delete this submission.");

        // Check deadline
        var assignment = await _assignmentRepository.GetByIdAsync(
            submission.AssignmentId,
            cancellationToken);

        if (assignment is not null && assignment.DueDate.HasValue && DateTime.UtcNow >= assignment.DueDate.Value)
            throw new ForbiddenException(
                "The submission deadline for this assignment has passed. Submissions cannot be deleted after the deadline.");

        await _submissionRepository.DeleteAsync(
            submission,
            cancellationToken);
    }
}