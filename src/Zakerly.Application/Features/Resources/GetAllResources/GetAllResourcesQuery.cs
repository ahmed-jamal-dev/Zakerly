using MediatR;

namespace Zakerly.Application.Features.Resources.GetAllResources;

public sealed record GetAllResourcesQuery(
    Guid LessonId)
    : IRequest<List<GetAllResourcesResponse>>;