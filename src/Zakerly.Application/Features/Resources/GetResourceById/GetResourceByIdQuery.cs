using MediatR;

namespace Zakerly.Application.Features.Resources.GetResourceById;

public sealed record GetResourceByIdQuery(
    Guid ResourceId)
    : IRequest<GetResourceByIdResponse>;