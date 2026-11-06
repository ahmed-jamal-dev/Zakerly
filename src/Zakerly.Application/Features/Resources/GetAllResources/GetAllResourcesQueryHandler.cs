using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.GetAllResources;

public class GetAllResourcesQueryHandler
    : IRequestHandler<GetAllResourcesQuery, List<GetAllResourcesResponse>>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllResourcesQueryHandler(
        IResourceRepository resourceRepository,
        ILessonRepository lessonRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _resourceRepository = resourceRepository;
        _lessonRepository = lessonRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<GetAllResourcesResponse>> Handle(
        GetAllResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdWithCourseAsync(
            request.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(lesson),
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
                    nameof(lesson),
                    request.LessonId);
        }

        var resources = await _resourceRepository.GetAllByLessonIdAsync(
            request.LessonId,
            cancellationToken);

        return resources
            .Select(x => new GetAllResourcesResponse(
                x.Id,
                x.Name,
                x.FilePath))
            .ToList();
    }
}