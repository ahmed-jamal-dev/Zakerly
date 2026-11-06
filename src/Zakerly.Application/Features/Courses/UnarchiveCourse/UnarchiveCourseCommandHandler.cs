using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.UnarchiveCourse;

public class UnarchiveCourseCommandHandler
    : IRequestHandler<UnarchiveCourseCommand, UnarchiveCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public UnarchiveCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UnarchiveCourseResponse> Handle(
        UnarchiveCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(course),
                request.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException("You are not allowed to unarchive this course.");

        course.Unarchive();

        await _courseRepository.UpdateAsync(
            course,
            cancellationToken);

        return new UnarchiveCourseResponse(
            course.Id,
            course.Title,
            course.Description,
            course.IsPublished,
            course.IsArchived);
    }
}