using Application.DTOs.Task;
using MediatR;
using Domain.Entities;


namespace Application.Commands.CreateTask;

public class CreateTaskCommand : IRequest<TaskResponseDto>
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; }
    public int? CategoryId { get; set; }
    public List<int> TagIds { get; set; } = new();
}