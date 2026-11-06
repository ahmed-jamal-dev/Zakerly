using Zakerly.Domain.Enums;
namespace Zakerly.Domain.Entities;
public class Course : BaseEntity
{
    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public Guid InstructorId { get; private set; }

    public bool IsPublished { get; private set; }

    public bool IsArchived { get; private set; }

    // Navigation Properties

    public User Instructor { get; private set; } = null!;

    public ICollection<Lesson> Lessons { get; private set; } = [];

    public ICollection<Assignment> Assignments { get; private set; } = [];

    public ICollection<Enrollment> Enrollments { get; private set; } = [];

    private Course()
    {
    }

    public Course(

        string title,

        string description,

        Guid instructorId)

    {

        Title = title;

        Description = description;

        InstructorId = instructorId;

        IsPublished = false;
        IsArchived = false;
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

    public void Archive()
    {
        IsArchived = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unarchive()
    {
        IsArchived = false;
        UpdatedAt = DateTime.UtcNow;
    }
}