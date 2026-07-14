using Domain.Entities;

namespace Application.Interfaces;

public interface ITaskRepository
{
    Task<UserTask?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<UserTask>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task AddAsync(UserTask task, CancellationToken ct = default);
    Task UpdateAsync(UserTask task, CancellationToken ct = default);
    Task DeleteAsync(UserTask task, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}