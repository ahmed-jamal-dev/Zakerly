using MediatR;

namespace Zakerly.Application.Features.Courses.GetAllCourses;

public sealed record GetAllCoursesQuery
    : IRequest<List<GetAllCoursesResponse>>;