namespace Zakerly.Application.Features.Resources.CreateResource;

public sealed record CreateResourceResponse(
    Guid ResourceId,
    string Name,
    string FilePath);