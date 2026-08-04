namespace Zakerly.API.Contracts.Lessons;

public sealed record CreateLessonRequest(
    string Title,
    string Content);