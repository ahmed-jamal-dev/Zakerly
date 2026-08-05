using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.GetAssignmentById;

public class GetAssignmentByIdQueryHandler
    : IRequestHandler<GetAssignmentByIdQuery, GetAssignmentByIdResponse>
{
    private readonly IAssignmentRepository _assignmentRepository;

    public GetAssignmentByIdQueryHandler(
        IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task<GetAssignmentByIdResponse> Handle(
        GetAssignmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(
            request.AssignmentId,
            cancellationToken);

        if (assignment is null)
            throw new NotFoundException(
                nameof(Assignment),
                request.AssignmentId);

        return new GetAssignmentByIdResponse(
            assignment.Id,
            assignment.Title,
            assignment.Description,
            assignment.CourseId);
    }
}