using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Lessons.MarkLessonAsCompleted;

public class MarkLessonAsCompletedCommandHandler
    : IRequestHandler<
        MarkLessonAsCompletedCommand,
        MarkLessonAsCompletedResponse>
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkLessonAsCompletedCommandHandler(
        ILessonRepository lessonRepository,
        IEnrollmentRepository enrollmentRepository,
        ILessonProgressRepository lessonProgressRepository,
        ICurrentUserService currentUserService)
    {
        _lessonRepository = lessonRepository;
        _enrollmentRepository = enrollmentRepository;
        _lessonProgressRepository = lessonProgressRepository;
        _currentUserService = currentUserService;
    }

    public async Task<MarkLessonAsCompletedResponse> Handle(
        MarkLessonAsCompletedCommand request,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(
            request.LessonId,
            cancellationToken);

        if (lesson is null)
            throw new NotFoundException(
                nameof(Lesson),
                request.LessonId);

        var isEnrolled = await _enrollmentRepository.ExistsAsync(
            _currentUserService.UserId,
            lesson.CourseId,
            cancellationToken);

        if (!isEnrolled)
            throw new ForbiddenException(
                "You are not enrolled in this course.");

        var progress = await _lessonProgressRepository
            .GetByStudentAndLessonAsync(
                _currentUserService.UserId,
                lesson.Id,
                cancellationToken);

        if (progress is not null)
        {
            return new MarkLessonAsCompletedResponse(
                progress.Id,
                progress.LessonId,
                progress.IsCompleted,
                progress.CompletedAt);
        }

        progress = new LessonProgress(
            _currentUserService.UserId,
            lesson.Id);

        progress.MarkAsCompleted();

        await _lessonProgressRepository.AddAsync(
            progress,
            cancellationToken);

        return new MarkLessonAsCompletedResponse(
            progress.Id,
            progress.LessonId,
            progress.IsCompleted,
            progress.CompletedAt);
    }
}