using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public class GetAllLessonsQueryHandler
    : IRequestHandler<GetAllLessonsQuery, List<GetAllLessonsResponse>>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllLessonsQueryHandler(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<GetAllLessonsResponse>> Handle(
        GetAllLessonsQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(course),
                request.CourseId);

        if (course.IsArchived)
        {
            bool isOwner = course.InstructorId == _currentUserService.UserId;
            bool isEnrolled = await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                course.Id,
                cancellationToken);

            if (!isOwner && !isEnrolled)
                throw new NotFoundException(
                    nameof(course),
                    request.CourseId);
        }

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