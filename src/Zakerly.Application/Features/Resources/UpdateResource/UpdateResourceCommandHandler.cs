using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.UpdateResource;

public class UpdateResourceCommandHandler
    : IRequestHandler<UpdateResourceCommand, UpdateResourceResponse>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateResourceCommandHandler(
        IResourceRepository resourceRepository,
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _resourceRepository = resourceRepository;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateResourceResponse> Handle(
        UpdateResourceCommand request,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(
            request.ResourceId,
            cancellationToken);

        if (resource is null)
            throw new NotFoundException(
                nameof(Resource),
                request.ResourceId);

        var lesson = await _lessonRepository.GetByIdAsync(
            resource.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(Lesson),
                resource.LessonId);

        var course = await _courseRepository.GetByIdAsync(
            lesson.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                lesson.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to update this resource.");

        resource.Update(
            request.Name,
            request.FilePath);

        await _resourceRepository.UpdateAsync(
            resource,
            cancellationToken);

        return new UpdateResourceResponse(
            resource.Id,
            resource.Name,
            resource.FilePath);
    }
}