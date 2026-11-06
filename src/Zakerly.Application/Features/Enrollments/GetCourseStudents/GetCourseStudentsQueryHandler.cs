using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.GetCourseStudents;

public class GetCourseStudentsQueryHandler
    : IRequestHandler<GetCourseStudentsQuery, PagedResponse<GetCourseStudentsResponse>>
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

    public async Task<PagedResponse<GetCourseStudentsResponse>> Handle(
        GetCourseStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

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

        var totalItems = await _enrollmentRepository.CountByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        var enrollments = await _enrollmentRepository.GetByCourseIdPagedAsync(
            request.CourseId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = enrollments
            .Select(x => new GetCourseStudentsResponse(
                x.Student.Id,
                x.Student.FullName,
                x.Student.Email,
                x.CreatedAt))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetCourseStudentsResponse>(items, metadata);
    }
}