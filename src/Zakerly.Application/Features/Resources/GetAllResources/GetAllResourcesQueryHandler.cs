using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.GetAllResources;

public class GetAllResourcesQueryHandler
    : IRequestHandler<GetAllResourcesQuery, PagedResponse<GetAllResourcesResponse>>
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

    public async Task<PagedResponse<GetAllResourcesResponse>> Handle(
        GetAllResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

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

        var totalItems = await _resourceRepository.CountByLessonIdAsync(
            request.LessonId,
            cancellationToken);

        var resources = await _resourceRepository.GetByLessonIdPagedAsync(
            request.LessonId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = resources
            .Select(x => new GetAllResourcesResponse(
                x.Id,
                x.Name,
                x.FilePath))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetAllResourcesResponse>(items, metadata);
    }
}