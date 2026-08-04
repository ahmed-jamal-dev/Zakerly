using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.DeleteCourse;

public class DeleteCourseCommandHandler
    : IRequestHandler<DeleteCourseCommand>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new Exception("Course not found.");

        if (course.InstructorId != _currentUserService.UserId)
            throw new Exception("You are not allowed to delete this course.");

        await _courseRepository.DeleteAsync(
            course,
            cancellationToken);
    }
}