using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.GetCourseById;

public class GetCourseByIdQueryHandler
    : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResponse>
{
    private readonly ICourseRepository _courseRepository;

    public GetCourseByIdQueryHandler(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<GetCourseByIdResponse> Handle(
        GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(course),
                request.CourseId
                );
        
        return new GetCourseByIdResponse(
            course.Id,
            course.Title,
            course.Description,
            course.IsPublished,
            course.Instructor.FullName);
    }
}