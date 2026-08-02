namespace Zakerly.Application.Features.Courses.CreateCourse;
using MediatR;
public record CreateCourseCommand(
    string Title,
    string Description
) :  IRequest<CreateCourseResponse>;