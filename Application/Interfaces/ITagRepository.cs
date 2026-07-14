using Domain.Entities;

namespace Application.Interfaces;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Tag>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken ct = default);
    Task AddAsync(Tag tag, CancellationToken ct = default);
    Task UpdateAsync(Tag tag, CancellationToken ct = default);
    Task DeleteAsync(Tag tag, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, int userId, CancellationToken ct = default);
}