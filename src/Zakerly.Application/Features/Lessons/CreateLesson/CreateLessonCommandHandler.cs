using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.CreateLesson;

public class CreateLessonCommandHandler
    : IRequestHandler<CreateLessonCommand, CreateLessonResponse>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateLessonCommandHandler(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CreateLessonResponse> Handle(
        CreateLessonCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);
        if (course is null)
        {
            throw new NotFoundException(
                nameof(Course),
                request.CourseId);
        }
        if (course.InstructorId != _currentUserService.UserId)
        {
            throw new ForbiddenException(
                "You are not allowed to add lessons to this course.");
        }
        var lesson = new Lesson(
            request.Title,
            request.Content,
            request.CourseId);
        await _lessonRepository.AddAsync(
            lesson,
            cancellationToken);
        
    return new CreateLessonResponse(

        lesson.Id,

        lesson.Title,

        lesson.Content,

        lesson.CourseId);
    }
}