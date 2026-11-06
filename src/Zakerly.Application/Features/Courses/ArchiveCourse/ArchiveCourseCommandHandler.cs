using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.ArchiveCourse;

public class ArchiveCourseCommandHandler
    : IRequestHandler<ArchiveCourseCommand, ArchiveCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public ArchiveCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ArchiveCourseResponse> Handle(
        ArchiveCourseCommand request,
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
            throw new ForbiddenException("You are not allowed to archive this course.");

        course.Archive();

        await _courseRepository.UpdateAsync(
            course,
            cancellationToken);

        return new ArchiveCourseResponse(
            course.Id,
            course.Title,
            course.Description,
            course.IsPublished,
            course.IsArchived);
    }
}