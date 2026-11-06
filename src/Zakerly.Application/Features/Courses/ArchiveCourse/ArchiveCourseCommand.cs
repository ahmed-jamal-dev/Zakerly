using MediatR;

namespace Zakerly.Application.Features.Courses.ArchiveCourse;

public sealed record ArchiveCourseCommand(Guid CourseId) : IRequest<ArchiveCourseResponse>;