using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.GetCourseStudents;

public class GetCourseStudentsQueryHandler
    : IRequestHandler<GetCourseStudentsQuery, List<GetCourseStudentsResponse>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCourseStudentsQueryHandler(
        IEnrollmentRepository enrollmentRepository,
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<GetCourseStudentsResponse>> Handle(
        GetCourseStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(Course),
                request.CourseId);

        if (course.InstructorId != _currentUserService.UserId)
            throw new ForbiddenException(
                "You are not allowed to view students in this course.");

        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        return enrollments
            .Select(x => new GetCourseStudentsResponse(
                x.Student.Id,
                x.Student.FullName,
                x.Student.Email,
                x.CreatedAt))
            .ToList();
    }
}