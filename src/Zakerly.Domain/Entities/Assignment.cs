namespace Zakerly.Domain.Entities;

public class Assignment : BaseEntity
{

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public Guid CourseId { get; private set; }
    
    // Navigation Properties
    public Course Course { get; private set; } = null!;

    public ICollection<Submission> Submissions { get; private set; } = [];

    private Assignment()
    {
    }

    public Assignment(string title, string description, Guid courseId)
    {
        Title = title;
        Description = description;
        CourseId = courseId;
        CreatedAt = DateTime.UtcNow;
    }
    public void Update(
        string title,
        string description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}