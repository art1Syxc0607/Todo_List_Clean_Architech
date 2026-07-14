using Domain.Exceptions;

namespace Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? Color { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User? User { get; private set; }
    public ICollection<UserTask> Tasks { get; private set; } = new List<UserTask>();

    public Category(string name, int userId, string? description = null, string? color = null)
    {
        SetName(name);
        UserId = userId;
        Description = description;
        Color = color;
        CreatedAt = DateTime.UtcNow;
    }

    private Category() { }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Название категории не может быть пустым");
        Name = name;
    }
}


