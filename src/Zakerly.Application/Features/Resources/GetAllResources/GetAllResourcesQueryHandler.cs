using MediatR;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.GetAllResources;

public class GetAllResourcesQueryHandler
    : IRequestHandler<GetAllResourcesQuery, List<GetAllResourcesResponse>>
{
    private readonly IResourceRepository _resourceRepository;

    public GetAllResourcesQueryHandler(
        IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<List<GetAllResourcesResponse>> Handle(
        GetAllResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var resources = await _resourceRepository.GetAllByLessonIdAsync(
            request.LessonId,
            cancellationToken);

        return resources
            .Select(x => new GetAllResourcesResponse(
                x.Id,
                x.Name,
                x.FilePath))
            .ToList();
    }
}