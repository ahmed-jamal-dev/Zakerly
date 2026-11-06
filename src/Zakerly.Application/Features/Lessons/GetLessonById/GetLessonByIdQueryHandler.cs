using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.GetLessonById;

public class GetLessonByIdQueryHandler
    : IRequestHandler<GetLessonByIdQuery, GetLessonByIdResponse>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetLessonByIdQueryHandler(
        ILessonRepository lessonRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _lessonRepository = lessonRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetLessonByIdResponse> Handle(
        GetLessonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdWithCourseAsync(
            request.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(Lesson),
                request.LessonId);

        var course = lesson.Course;

        if (course.IsArchived)
        {
            bool isOwner = course.InstructorId == _currentUserService.UserId;
            bool isEnrolled = await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                course.Id,
                cancellationToken);

            if (!isOwner && !isEnrolled)
                throw new NotFoundException(
                    nameof(Lesson),
                    request.LessonId);
        }

        return new GetLessonByIdResponse(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            lesson.CourseId);
    }
}