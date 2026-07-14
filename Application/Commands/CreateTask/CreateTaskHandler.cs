using Application.DTOs.Task;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Exceptions;

namespace Application.Commands.CreateTask;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskResponseDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskHandler(
        ITaskRepository taskRepository,
        ICategoryRepository categoryRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<TaskResponseDto> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        // 1. Проверка категории
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, ct);
            if (category == null)
                throw new DomainException($"Категория {request.CategoryId} не найдена");
            if (category.UserId != request.UserId)
                throw new UnauthorizedAccessException("Нет доступа к категории");
        }

        // 2. Проверка тегов
        var tags = new List<Tag>();
        if (request.TagIds.Any())
        {
            tags = await _tagRepository.GetByIdsAsync(request.TagIds, ct);
            if (tags.Count != request.TagIds.Count)
                throw new DomainException("Некоторые теги не найдены");
            if (tags.Any(t => t.UserId != request.UserId))
                throw new UnauthorizedAccessException("Нет доступа к некоторым тегам");
        }

        // 3. Создание задачи
        var task = new Domain.Entities.UserTask(
            request.Title,
            request.UserId,
            request.Description,
            request.DueDate);

        // 4. Установка категории и тегов
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, ct);
            task.SetCategory(category);
        }

        foreach (var tag in tags)
            task.AddTag(tag);

        // 5. Сохранение
        await _taskRepository.AddAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // 6. Ответ
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            Status = task.Status,
            Priority = request.Priority,
            CategoryId = task.CategoryId,
            Tags = task.Tags.Select(t => new TagResponseDto { Id = t.Id, Name = t.Name }).ToList()
        };
    }
}