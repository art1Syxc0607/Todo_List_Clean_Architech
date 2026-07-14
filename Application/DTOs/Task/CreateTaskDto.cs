using Domain.Entities;


namespace Application.DTOs.Task;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public int? CategoryId { get; set; }
    public List<int> TagIds { get; set; } = new();
}