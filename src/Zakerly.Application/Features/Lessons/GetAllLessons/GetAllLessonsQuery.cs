using MediatR;

namespace Zakerly.Application.Features.Lessons.GetAllLessons;

public sealed record GetAllLessonsQuery(
    Guid CourseId)
    : IRequest<List<GetAllLessonsResponse>>;