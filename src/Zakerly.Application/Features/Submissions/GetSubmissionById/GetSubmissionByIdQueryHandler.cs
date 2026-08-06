using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GetSubmissionById;

public class GetSubmissionByIdQueryHandler
    : IRequestHandler<GetSubmissionByIdQuery, GetSubmissionByIdResponse>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetSubmissionByIdQueryHandler(
        ISubmissionRepository submissionRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetSubmissionByIdResponse> Handle(
        GetSubmissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var submission = await _submissionRepository.GetByIdAsync(
            request.SubmissionId,
            cancellationToken);

        if (submission is null)
            throw new NotFoundException(
                nameof(Submission),
                request.SubmissionId);

        if (_currentUserService.Role == UserRole.Student)
        {
            if (submission.StudentId != _currentUserService.UserId)
                throw new ForbiddenException(
                    "You are not allowed to view this submission.");
        }
        else if (_currentUserService.Role == UserRole.Instructor)
        {
            var course = await _courseRepository.GetByIdAsync(
                submission.Assignment.CourseId,
                cancellationToken);

            if (course is null)
                throw new NotFoundException(
                    nameof(Course),
                    submission.Assignment.CourseId);

            if (course.InstructorId != _currentUserService.UserId)
                throw new ForbiddenException(
                    "You are not allowed to view this submission.");
        }

        return new GetSubmissionByIdResponse(
            submission.Id,
            submission.AssignmentId,
            submission.StudentId,
            submission.Student.FullName,
            submission.Student.Email,
            submission.FilePath,
            submission.Grade,
            submission.Feedback,
            submission.CreatedAt);
    }
}