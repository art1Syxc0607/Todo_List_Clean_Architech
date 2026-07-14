using Application.DTOs.Task;
using MediatR;

namespace Application.Queries.GetTask;

public class GetTaskQuery : IRequest<TaskResponseDto>
{
    public int Id { get; set; }
    public int UserId { get; set; }  // ← проверяем, что задача принадлежит пользователю
}