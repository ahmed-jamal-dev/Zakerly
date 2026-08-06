namespace Zakerly.Application.Features.Resources.UpdateResource;

public sealed record UpdateResourceResponse(
    Guid ResourceId,
    string Name,
    string FilePath);