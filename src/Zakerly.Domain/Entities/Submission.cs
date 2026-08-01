namespace Zakerly.Domain.Entities;

public class Submission : BaseEntity
{

    public string FilePath { get; private set; } = string.Empty;

    public decimal? Grade { get; private set; }

    public string? Feedback { get; private set; }

    public Guid AssignmentId { get; private set; }

    public Guid StudentId { get; private set; }


    // Navigation Properties
    public Assignment Assignment { get; private set; } = null!;

    public User Student { get; private set; } = null!;

    private Submission()
    {
    }

    public Submission(string filePath, Guid assignmentId, Guid studentId)
    {
        FilePath = filePath;
        AssignmentId = assignmentId;
        StudentId = studentId;
        CreatedAt = DateTime.UtcNow;
    }
}