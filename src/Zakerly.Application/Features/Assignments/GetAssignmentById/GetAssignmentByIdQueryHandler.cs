using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.GetAssignmentById;

public class GetAssignmentByIdQueryHandler
    : IRequestHandler<GetAssignmentByIdQuery, GetAssignmentByIdResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAssignmentByIdQueryHandler(
        IAssignmentRepository assignmentRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetAssignmentByIdResponse> Handle(
        GetAssignmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdWithCourseAsync(
            request.AssignmentId,
            cancellationToken);

        if (assignment is null)
            throw new NotFoundException(
                nameof(Assignment),
                request.AssignmentId);

        var course = assignment.Course;

        if (course.IsArchived)
        {
            bool isOwner = course.InstructorId == _currentUserService.UserId;
            bool isEnrolled = await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                course.Id,
                cancellationToken);

            if (!isOwner && !isEnrolled)
                throw new NotFoundException(
                    nameof(Assignment),
                    request.AssignmentId);
        }

        return new GetAssignmentByIdResponse(
            assignment.Id,
            assignment.Title,
            assignment.Description,
            assignment.CourseId,
            assignment.DueDate);
    }
}