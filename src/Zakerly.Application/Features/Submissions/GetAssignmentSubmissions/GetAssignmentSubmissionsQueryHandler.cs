using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GetAssignmentSubmissions;

public class GetAssignmentSubmissionsQueryHandler
    : IRequestHandler<GetAssignmentSubmissionsQuery, List<GetAssignmentSubmissionsResponse>>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAssignmentSubmissionsQueryHandler(
        ISubmissionRepository submissionRepository,
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<GetAssignmentSubmissionsResponse>> Handle(
        GetAssignmentSubmissionsQuery request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(
            request.AssignmentId,
            cancellationToken);

        if (assignment is null)
            throw new NotFoundException(
                nameof(Assignment),
                request.AssignmentId);

        var course = await _courseRepository.GetByIdAsync(
            assignment.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                assignment.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to view these submissions.");

        var submissions = await _submissionRepository.GetByAssignmentIdAsync(
            request.AssignmentId,
            cancellationToken);

        return submissions
            .Select(x => new GetAssignmentSubmissionsResponse(
                x.Id,
                x.StudentId,
                x.Student.FullName,
                x.Student.Email,
                x.FilePath,
                x.Grade,
                x.Feedback,
                x.CreatedAt))
            .ToList();
    }
}