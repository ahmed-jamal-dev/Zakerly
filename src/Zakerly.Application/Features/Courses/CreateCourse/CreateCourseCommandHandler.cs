using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Courses.CreateCourse;

public class CreateCourseCommandHandler
    : IRequestHandler<CreateCourseCommand, CreateCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUser;

    public CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUser)
    {
        _courseRepository = courseRepository;
        _currentUser = currentUser;
    }

    public async Task<CreateCourseResponse> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedException(
                "Authentication is required to create a course.");
        }

        if (_currentUser.Role != UserRole.Instructor)
        {
            throw new ForbiddenException(
                "Only instructors can create courses.");
        }
        var course = new Course(
            request.Title,
            request.Description,
            _currentUser.UserId);

        await _courseRepository.AddAsync(course, cancellationToken);

        return new CreateCourseResponse(
            course.Id,
            course.Title,
            course.IsPublished);
    }
}