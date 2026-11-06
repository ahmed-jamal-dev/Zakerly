using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.GetMyEnrollments;

public class GetMyEnrollmentsQueryHandler
    : IRequestHandler<GetMyEnrollmentsQuery, PagedResponse<GetMyEnrollmentsResponse>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyEnrollmentsQueryHandler(
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResponse<GetMyEnrollmentsResponse>> Handle(
        GetMyEnrollmentsQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

        var totalItems = await _enrollmentRepository.CountByStudentIdAsync(
            _currentUserService.UserId,
            cancellationToken);

        var enrollments = await _enrollmentRepository.GetByStudentIdPagedAsync(
            _currentUserService.UserId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = enrollments
            .Select(x => new GetMyEnrollmentsResponse(
                x.Id,
                x.CourseId,
                x.Course.Title,
                x.Course.IsPublished,
                x.Course.IsArchived))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetMyEnrollmentsResponse>(items, metadata);
    }
}