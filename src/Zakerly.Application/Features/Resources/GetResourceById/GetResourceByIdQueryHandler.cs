using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Resources.GetResourceById;

public class GetResourceByIdQueryHandler
    : IRequestHandler<GetResourceByIdQuery, GetResourceByIdResponse>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetResourceByIdQueryHandler(
        IResourceRepository resourceRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _resourceRepository = resourceRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetResourceByIdResponse> Handle(
        GetResourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdWithLessonCourseAsync(
            request.ResourceId,
            cancellationToken);

        if (resource is null)
            throw new NotFoundException(
                nameof(Resource),
                request.ResourceId);

        var course = resource.Lesson.Course;

        if (course.IsArchived)
        {
            bool isOwner = course.InstructorId == _currentUserService.UserId;
            bool isEnrolled = await _enrollmentRepository.ExistsAsync(
                _currentUserService.UserId,
                course.Id,
                cancellationToken);

            if (!isOwner && !isEnrolled)
                throw new NotFoundException(
                    nameof(Resource),
                    request.ResourceId);
        }

        return new GetResourceByIdResponse(
            resource.Id,
            resource.Name,
            resource.FilePath,
            resource.LessonId);
    }
}