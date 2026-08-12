namespace Zakerly.Domain.Entities;

public class LessonProgress : BaseEntity
{
    public Guid StudentId { get; private set; }

    public Guid LessonId { get; private set; }

    public bool IsCompleted { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    // Navigation Properties
    public User Student { get; private set; } = null!;

    public Lesson Lesson { get; private set; } = null!;

    private LessonProgress()
    {
    }

    public LessonProgress(
        Guid studentId,
        Guid lessonId)
    {
        StudentId = studentId;
        LessonId = lessonId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}