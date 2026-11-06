using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public class GetAllLessonsQueryHandler
    : IRequestHandler<GetAllLessonsQuery, PagedResponse<GetAllLessonsResponse>>
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

    public async Task<PagedResponse<GetAllLessonsResponse>> Handle(
        GetAllLessonsQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

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

        var totalItems = await _lessonRepository.CountByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        var lessons = await _lessonRepository.GetByCourseIdPagedAsync(
            request.CourseId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = lessons
            .Select(x => new GetAllLessonsResponse(
                x.Id,
                x.Title,
                x.Content))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetAllLessonsResponse>(items, metadata);
    }
}