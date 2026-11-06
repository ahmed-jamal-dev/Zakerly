using MediatR;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.GetAllCourses;

public class GetAllCoursesQueryHandler
    : IRequestHandler<GetAllCoursesQuery, PagedResponse<GetAllCoursesResponse>>
{
    private readonly ICourseRepository _courseRepository;

    public GetAllCoursesQueryHandler(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<PagedResponse<GetAllCoursesResponse>> Handle(
        GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

        var totalItems = await _courseRepository.CountPublishedNonArchivedAsync(cancellationToken);

        var courses = await _courseRepository.GetPublishedNonArchivedPagedAsync(
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = courses
            .Select(course =>
                new GetAllCoursesResponse(
                    course.Id,
                    course.Title,
                    course.Description,
                    course.IsPublished,
                    course.Instructor.FullName))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetAllCoursesResponse>(items, metadata);
    }
}