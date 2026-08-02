using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.UpdateCourse;

public class UpdateCourseCommandHandler
    : IRequestHandler<UpdateCourseCommand, UpdateCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateCourseResponse> Handle(
        UpdateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new Exception("Course not found.");

        if (course.InstructorId != _currentUserService.UserId)
            throw new Exception("You are not allowed to update this course.");

        course.Update(
            request.Title,
            request.Description);

        await _courseRepository.UpdateAsync(
            course,
            cancellationToken);

        return new UpdateCourseResponse(
            course.Id,
            course.Title,
            course.Description,
            course.IsPublished);
    }
}