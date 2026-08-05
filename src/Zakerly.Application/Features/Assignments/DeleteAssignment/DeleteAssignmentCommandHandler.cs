using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.DeleteAssignment;

public class DeleteAssignmentCommandHandler
    : IRequestHandler<DeleteAssignmentCommand>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteAssignmentCommand request,
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
                "You are not allowed to delete this assignment.");

        await _assignmentRepository.DeleteAsync(
            assignment,
            cancellationToken);
    }
}