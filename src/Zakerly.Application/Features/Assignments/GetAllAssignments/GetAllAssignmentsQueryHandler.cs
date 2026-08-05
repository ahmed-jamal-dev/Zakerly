using MediatR;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public class GetAllAssignmentsQueryHandler
    : IRequestHandler<GetAllAssignmentsQuery, List<GetAllAssignmentsResponse>>
{
    private readonly IAssignmentRepository _assignmentRepository;

    public GetAllAssignmentsQueryHandler(
        IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task<List<GetAllAssignmentsResponse>> Handle(
        GetAllAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
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