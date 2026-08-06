using MediatR;

namespace Zakerly.Application.Features.Submissions.GetMySubmissions;

public sealed record GetMySubmissionsQuery
    : IRequest<List<GetMySubmissionsResponse>>;