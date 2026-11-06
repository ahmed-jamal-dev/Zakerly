using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public class GetAllAssignmentsQueryHandler
    : IRequestHandler<GetAllAssignmentsQuery, List<GetAllAssignmentsResponse>>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllAssignmentsQueryHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<GetAllAssignmentsResponse>> Handle(
        GetAllAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(course),
                request.CourseId);

        if (course.IsArchived)
        {
            bool isOwner = course.InstructorId == _currentUserService.UserId;
            bool isEnrolled = await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                course.Id,
                cancellationToken);

            if (!isOwner && !isEnrolled)
                throw new NotFoundException(
                    nameof(course),
                    request.CourseId);
        }

        var assignments = await _assignmentRepository.GetAllByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        return assignments
            .Select(x => new GetAllAssignmentsResponse(
                x.Id,
                x.Title,
                x.Description))
            .ToList();
    }
}