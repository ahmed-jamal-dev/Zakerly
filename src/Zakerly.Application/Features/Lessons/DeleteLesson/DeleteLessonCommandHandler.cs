using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.DeleteLesson;

public class DeleteLessonCommandHandler
    : IRequestHandler<DeleteLessonCommand>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteLessonCommandHandler(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteLessonCommand request,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(
            request.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(Lesson),
                request.LessonId);

        var course = await _courseRepository.GetByIdAsync(
            lesson.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                lesson.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to delete this lesson.");

        await _lessonRepository.DeleteAsync(
            lesson,
            cancellationToken);
    }
}