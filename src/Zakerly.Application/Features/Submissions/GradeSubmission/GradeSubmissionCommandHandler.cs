using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GradeSubmission;

public class GradeSubmissionCommandHandler
    : IRequestHandler<GradeSubmissionCommand, GradeSubmissionResponse>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public GradeSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _courseRepository = courseRepository;
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

        return new GradeSubmissionResponse(
            submission.Id,
            submission.Grade!.Value,
            submission.Feedback);
    }
}