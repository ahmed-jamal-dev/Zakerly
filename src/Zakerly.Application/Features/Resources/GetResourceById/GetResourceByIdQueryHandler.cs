using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.GetResourceById;

public class GetResourceByIdQueryHandler
    : IRequestHandler<GetResourceByIdQuery, GetResourceByIdResponse>
{
    private readonly IResourceRepository _resourceRepository;

    public GetResourceByIdQueryHandler(
        IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<GetResourceByIdResponse> Handle(
        GetResourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(
            request.ResourceId,
            cancellationToken);

        if (resource is null)
            throw new NotFoundException(
                nameof(Resource),
                request.ResourceId);

        return new GetResourceByIdResponse(
            resource.Id,
            resource.Name,
            resource.FilePath,
            resource.LessonId);
    }
}