namespace Zakerly.Application.Features.Resources.GetResourceById;

public sealed record GetResourceByIdResponse(
    Guid ResourceId,
    string Name,
    string FilePath,
    Guid LessonId);