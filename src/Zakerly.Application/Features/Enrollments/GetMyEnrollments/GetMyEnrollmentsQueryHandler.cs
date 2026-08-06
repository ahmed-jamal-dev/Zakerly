using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Enrollments.GetMyEnrollments;

public class GetMyEnrollmentsQueryHandler
    : IRequestHandler<GetMyEnrollmentsQuery, List<GetMyEnrollmentsResponse>>
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

    public async Task<List<GetMyEnrollmentsResponse>> Handle(
        GetMyEnrollmentsQuery request,
        CancellationToken cancellationToken)
    {
        var enrollments = await _enrollmentRepository
            .GetByStudentIdAsync(
                _currentUserService.UserId,
                cancellationToken);

        return enrollments
            .Select(x => new GetMyEnrollmentsResponse(
                x.Id,
                x.CourseId,
                x.Course.Title,
                x.Course.IsPublished))
            .ToList();
    }
}