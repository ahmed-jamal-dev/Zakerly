using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.GetLessonById;

public class GetLessonByIdQueryHandler
    : IRequestHandler<GetLessonByIdQuery, GetLessonByIdResponse>
{
    private readonly ILessonRepository _lessonRepository;

    public GetLessonByIdQueryHandler(
        ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<GetLessonByIdResponse> Handle(
        GetLessonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(
            request.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(Lesson),
                request.LessonId);

        return new GetLessonByIdResponse(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            lesson.CourseId);
    }
}