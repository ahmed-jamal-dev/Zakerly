using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Application.Common.Pagination;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GetMySubmissions;

public class GetMySubmissionsQueryHandler
    : IRequestHandler<GetMySubmissionsQuery, PagedResponse<GetMySubmissionsResponse>>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMySubmissionsQueryHandler(
        ISubmissionRepository submissionRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResponse<GetMySubmissionsResponse>> Handle(
        GetMySubmissionsQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();

        var totalItems = await _submissionRepository.CountByStudentIdAsync(
            _currentUserService.UserId,
            cancellationToken);

        var submissions = await _submissionRepository.GetByStudentIdPagedAsync(
            _currentUserService.UserId,
            pagination.Skip,
            pagination.Take,
            cancellationToken);

        var items = submissions
            .Select(x => new GetMySubmissionsResponse(
                x.Id,
                x.AssignmentId,
                x.Assignment.Title,
                x.FilePath,
                x.Grade,
                x.Feedback,
                x.CreatedAt))
            .ToList();

        var metadata = pagination.CreateMetadata(totalItems);

        return new PagedResponse<GetMySubmissionsResponse>(items, metadata);
    }
}