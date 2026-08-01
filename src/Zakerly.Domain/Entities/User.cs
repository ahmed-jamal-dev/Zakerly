using Zakerly.Domain.Enums;

namespace Zakerly.Domain.Entities;
public class User : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;  
    public UserRole Role { get; private set; }    
    // Navigation Properties

    public ICollection<Course> Courses { get; private set; } = [];

    public ICollection<Enrollment> Enrollments { get; private set; } = [];

    public ICollection<Submission> Submissions { get; private set; } = [];
    private User()
    {
        
    }

    public User(string fullName, string email, string passwordHash, UserRole role)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}