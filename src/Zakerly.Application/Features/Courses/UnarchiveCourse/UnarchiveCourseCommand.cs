using MediatR;

namespace Zakerly.Application.Features.Courses.UnarchiveCourse;

public sealed record UnarchiveCourseCommand(Guid CourseId) : IRequest<UnarchiveCourseResponse>;