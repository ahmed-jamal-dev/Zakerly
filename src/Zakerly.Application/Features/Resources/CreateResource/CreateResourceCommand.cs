using MediatR;

namespace Zakerly.Application.Features.Resources.CreateResource;

public sealed record CreateResourceCommand(
    Guid LessonId,
    string Name,
    string FilePath)
    : IRequest<CreateResourceResponse>;