using MediatR;

namespace Zakerly.Application.Features.Enrollments.GetMyEnrollments;

public sealed record GetMyEnrollmentsQuery
    : IRequest<List<GetMyEnrollmentsResponse>>;