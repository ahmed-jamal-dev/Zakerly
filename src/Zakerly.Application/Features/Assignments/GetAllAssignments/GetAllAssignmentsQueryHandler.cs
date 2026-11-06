using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Assignments.GetAllAssignments;

public class GetAllAssignmentsQueryHandler
    : IRequestHandler<GetAllAssignmentsQuery, PagedResponse<GetAllAssignmentsResponse>>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllAssignmentsQueryHandler(
        IAssignmentRepository assignmentRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _assignmentRepository = assignmentRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResponse<GetAllAssignmentsResponse>> Handle(
        GetAllAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

        var course = await _courseRepository.GetByIdAsync(
            request.CourseId,
            cancellationToken);

        if (course is null)
            throw new NotFoundException(
                nameof(course),
                request.CourseId);

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

        var totalItems = await _assignmentRepository.CountByCourseIdAsync(
            request.CourseId,
            cancellationToken);

        var assignments = await _assignmentRepository.GetByCourseIdPagedAsync(
            request.CourseId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = assignments
            .Select(x => new GetAllAssignmentsResponse(
                x.Id,
                x.Title,
                x.Description))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetAllAssignmentsResponse>(items, metadata);
    }
}