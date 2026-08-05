using MediatR;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public class GetAllLessonsQueryHandler
    : IRequestHandler<GetAllLessonsQuery, List<GetAllLessonsResponse>>
{
    private readonly ILessonRepository _lessonRepository;

    public GetAllLessonsQueryHandler(
        ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<List<GetAllLessonsResponse>> Handle(
        GetAllLessonsQuery request,
        CancellationToken cancellationToken)
    {
        var lessons = await _lessonRepository.GetAllByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        return lessons
            .Select(x => new GetAllLessonsResponse(
                x.Id,
                x.Title,
                x.Content))
            .ToList();
    }
}