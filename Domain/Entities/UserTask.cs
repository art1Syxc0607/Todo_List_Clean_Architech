using Domain.Exceptions;

namespace Domain.Entities;

public class UserTask
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }  
    public bool IsCompleted { get; private set; }
    public TaskStatus Status { get; private set; }
    public int UserId { get; private set; }
    public int? CategoryId { get; private set; }

    // Навигационные свойства (только для связи, без логики)
    public User? User { get; private set; }
    public Category? Category { get; private set; }
    public ICollection<Tag> Tags { get; private set; } = new List<Tag>();

    // Конструктор для создания задачи
    public UserTask(string title, int userId, string? description = null, DateTime? dueDate = null)
    {
        SetTitle(title);
        UserId = userId;
        Description = description;
        DueDate = dueDate;
        Status = TaskStatus.NotStarted;
        IsCompleted = false;
    }

    // Private constructor for EF Core
    private UserTask() { }

    // Бизнес-методы
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Название задачи не может быть пустым");
        Title = title;
    }

    public void SetDescription(string? description) => Description = description;

    public void SetDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value.Date < DateTime.Today)
            throw new DomainException("Дедлайн не может быть в прошлом");
        DueDate = dueDate;
    }

    public void MarkAsCompleted()
    {
        if (Status == TaskStatus.Completed)
            throw new DomainException("Задача уже выполнена");
        if (Status == TaskStatus.Cancelled)
            throw new DomainException("Отмененную задачу нельзя выполнить");

        Status = TaskStatus.Completed;
        IsCompleted = true;
    }

    public void Cancel()
    {
        if (Status == TaskStatus.Completed)
            throw new DomainException("Выполненную задачу нельзя отменить");
        Status = TaskStatus.Cancelled;
        IsCompleted = false;
    }

    public void SetStatus(TaskStatus newStatus)
    {
        if (newStatus == TaskStatus.Completed)
        {
            MarkAsCompleted();
            return;
        }
        if (newStatus == TaskStatus.Cancelled)
        {
            Cancel();
            return;
        }
        Status = newStatus;
    }

    public void AddTag(Tag tag)
    {
        if (Tags.All(t => t.Id != tag.Id))
            Tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        Tags.Remove(tag);
    }

    public void SetCategory(Category? category)
    {
        Category = category;
        CategoryId = category?.Id;
    }
}

