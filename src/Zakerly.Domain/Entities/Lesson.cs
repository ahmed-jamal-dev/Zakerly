namespace Zakerly.Domain.Entities;

public class Lesson : BaseEntity
{

    public string Title { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public Guid CourseId { get; private set; }
    
    // Navigation Properties
    public Course Course { get; private set; } = null!;

    public ICollection<Resource> Resources { get; private set; } = [];

    private Lesson()
    {
    }

    public Lesson(string title, string content, Guid courseId)
    {
        Title = title;
        Content = content;
        CourseId = courseId;
        CreatedAt = DateTime.UtcNow;
    }
    public void Update(

        string title,

        string content)

    {

        Title = title;

        Content = content;

        UpdatedAt = DateTime.UtcNow;

    }
}