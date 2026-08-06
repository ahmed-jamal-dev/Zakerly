namespace Zakerly.Application.Features.Resources.GetAllResources;

public sealed record GetAllResourcesResponse(
    Guid ResourceId,
    string Name,
    string FilePath);