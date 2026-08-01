namespace Zakerly.Domain.Entities;

public class Enrollment : BaseEntity
{

    public Guid StudentId { get; private set; }

    public Guid CourseId { get; private set; }



    // Navigation Properties
    public User Student { get; private set; } = null!;

    public Course Course { get; private set; } = null!;

    private Enrollment()
    {
    }

    public Enrollment(Guid studentId, Guid courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
        CreatedAt = DateTime.UtcNow;
    }
}