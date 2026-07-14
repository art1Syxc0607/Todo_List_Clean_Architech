using Domain.Exceptions;

namespace Domain.Entities;

public class Tag
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User? User { get; private set; }
    public ICollection<UserTask> Tasks { get; private set; } = new List<UserTask>();

    public Tag(string name, int userId)
    {
        SetName(name);
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    private Tag() { }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Название тега не может быть пустым");
        Name = name;
    }
}
