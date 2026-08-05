using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.UpdateAssignment;

public class UpdateAssignmentCommandHandler
    : IRequestHandler<UpdateAssignmentCommand, UpdateAssignmentResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateAssignmentResponse> Handle(
        UpdateAssignmentCommand request,
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
                "You are not allowed to update this assignment.");

        assignment.Update(
            request.Title,
            request.Description);

        await _assignmentRepository.UpdateAsync(
            assignment,
            cancellationToken);

        return new UpdateAssignmentResponse(
            assignment.Id,
            assignment.Title,
            assignment.Description);
    }
}