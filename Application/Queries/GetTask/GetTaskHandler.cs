using Application.DTOs.Task;
using Application.Interfaces;
using Application.Queries.GetTask;
using Domain.Exceptions;
using MediatR;


namespace Application.Queries.GetTask;

public class GetTaskHandler : IRequestHandler<GetTaskQuery, TaskResponseDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponseDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        // 1. Получаем задачу из репозитория (вместе с тегами)
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        // 2. Проверяем, существует ли задача
        if (task == null)
            throw new DomainException($"Задача с ID {request.Id} не найдена");

        // 3. Проверяем, принадлежит ли задача пользователю
        if (task.UserId != request.UserId)
            throw new UnauthorizedAccessException("Нет доступа к этой задаче");

        // 4. Преобразуем в DTO
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            Status = task.Status,
            //Priority = task.Priority,      // если есть поле Priority
            CategoryId = task.CategoryId,
            Tags = task.Tags.Select(t => new TagResponseDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        };
    }
}