using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Submissions.GetMySubmissions;

public class GetMySubmissionsQueryHandler
    : IRequestHandler<GetMySubmissionsQuery, List<GetMySubmissionsResponse>>
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

    public async Task<List<GetMySubmissionsResponse>> Handle(
        GetMySubmissionsQuery request,
        CancellationToken cancellationToken)
    {
        var submissions = await _submissionRepository.GetByStudentIdAsync(
            _currentUserService.UserId,
            cancellationToken);

        return submissions
            .Select(x => new GetMySubmissionsResponse(
                x.Id,
                x.AssignmentId,
                x.Assignment.Title,
                x.FilePath,
                x.Grade,
                x.Feedback,
                x.CreatedAt))
            .ToList();
    }
}