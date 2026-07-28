using Zakerly.Domain.Enums;

namespace Zakerly.Domain.Entities;
public class User
{
    public Guid Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;    
    public UserRole Role { get; private set; }    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

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