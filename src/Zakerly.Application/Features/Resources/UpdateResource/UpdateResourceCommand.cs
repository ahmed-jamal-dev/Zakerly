using MediatR;

namespace Zakerly.Application.Features.Resources.UpdateResource;

public sealed record UpdateResourceCommand(
    Guid ResourceId,
    string Name,
    string FilePath)
    : IRequest<UpdateResourceResponse>;