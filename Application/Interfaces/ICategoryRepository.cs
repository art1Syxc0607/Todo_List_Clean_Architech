using Domain.Entities;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Category>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    Task UpdateAsync(Category category, CancellationToken ct = default);
    Task DeleteAsync(Category category, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, int userId, CancellationToken ct = default);
}