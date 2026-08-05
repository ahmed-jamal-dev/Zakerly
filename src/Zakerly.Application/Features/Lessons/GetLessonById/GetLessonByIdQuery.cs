using MediatR;

namespace Zakerly.Application.Features.Lessons.GetLessonById;

public sealed record GetLessonByIdQuery(
    Guid LessonId)
    : IRequest<GetLessonByIdResponse>;