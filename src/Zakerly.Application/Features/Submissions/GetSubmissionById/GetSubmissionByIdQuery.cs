using MediatR;

namespace Zakerly.Application.Features.Submissions.GetSubmissionById;

public sealed record GetSubmissionByIdQuery(
    Guid SubmissionId)
    : IRequest<GetSubmissionByIdResponse>;