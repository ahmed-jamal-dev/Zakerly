using MediatR;

namespace Zakerly.Application.Features.Resources.DeleteResource;

public sealed record DeleteResourceCommand(
    Guid ResourceId)
    : IRequest;