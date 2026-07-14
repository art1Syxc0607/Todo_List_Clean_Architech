using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<UserTask> Tasks { get; private set; } = new List<UserTask>();
    public ICollection<Category> Categories { get; private set; } = new List<Category>();
    public ICollection<Tag> Tags { get; private set; } = new List<Tag>();

    public User(string email, string userName, string passwordHash)
    {
        Email = email;
        UserName = userName;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    private User() { }

    public void UpdateProfile(string? userName = null, string? email = null)
    {
        if (userName != null)
            UserName = userName;
        if (email != null)
            Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}
