using Domain.Entities;


namespace Application.DTOs.Task;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public Domain.Entities.TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public int? CategoryId { get; set; }
    public List<TagResponseDto> Tags { get; set; } = new();
}

public class TagResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}