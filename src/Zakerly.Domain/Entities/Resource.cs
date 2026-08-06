namespace Zakerly.Domain.Entities;

public class Resource: BaseEntity
{

    public string Name { get; private set; } = string.Empty;

    public string FilePath { get; private set; } = string.Empty;

    public Guid LessonId { get; private set; }
    
    // Navigation Property
    public Lesson Lesson { get; private set; } = null!;

    private Resource()
    {
    }

    public Resource(string name, string filePath, Guid lessonId)
    {
        Name = name;
        FilePath = filePath;
        LessonId = lessonId;
        CreatedAt = DateTime.UtcNow;
    }
    public void Update(
        string name,
        string filePath)
    {
        Name = name;
        FilePath = filePath;
        UpdatedAt = DateTime.UtcNow;
    }
}