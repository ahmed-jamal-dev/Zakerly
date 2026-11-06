using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.GetCourseById;

public class GetCourseByIdQueryHandler
    : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCourseByIdQueryHandler(
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
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

        return new GetCourseByIdResponse(
            course.Id,
            course.Title,
            course.Description,
            course.IsPublished,
            course.Instructor.FullName);
    }
}