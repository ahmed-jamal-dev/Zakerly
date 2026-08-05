using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.CreateAssignment;

public class CreateAssignmentCommandHandler
    : IRequestHandler<CreateAssignmentCommand, CreateAssignmentResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CreateAssignmentResponse> Handle(
        CreateAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                request.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to add assignments to this course.");

        var assignment = new Assignment(
            request.Title,
            request.Description,
            request.CourseId);

        await _assignmentRepository.AddAsync(
            assignment,
            cancellationToken);

        return new CreateAssignmentResponse(
            assignment.Id,
            assignment.Title,
            assignment.Description);
    }
}