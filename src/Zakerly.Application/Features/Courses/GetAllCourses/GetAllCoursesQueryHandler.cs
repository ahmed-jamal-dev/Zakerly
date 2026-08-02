using MediatR;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.GetAllCourses;

public class GetAllCoursesQueryHandler
    : IRequestHandler<GetAllCoursesQuery, List<GetAllCoursesResponse>>
{
    private readonly ICourseRepository _courseRepository;

    public GetAllCoursesQueryHandler(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<List<GetAllCoursesResponse>> Handle(
        GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync(
            cancellationToken);

        return courses.Select(course =>
                new GetAllCoursesResponse(
                    course.Id,
                    course.Title,
                    course.Description,
                    course.IsPublished,
                    course.Instructor.FullName))
            .ToList();
    }
}