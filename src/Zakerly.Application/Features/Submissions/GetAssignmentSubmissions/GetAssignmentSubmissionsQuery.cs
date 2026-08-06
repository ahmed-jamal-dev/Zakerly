using MediatR;

namespace Zakerly.Application.Features.Submissions.GetAssignmentSubmissions;

public sealed record GetAssignmentSubmissionsQuery(
    Guid AssignmentId)
    : IRequest<List<GetAssignmentSubmissionsResponse>>;