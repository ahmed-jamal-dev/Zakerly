namespace Zakerly.API.Contracts.Lessons;

public sealed record UpdateLessonRequest(
    string Title,
    string Content);