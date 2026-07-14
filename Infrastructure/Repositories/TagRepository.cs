using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
        => _context = context;

    public async Task<Tag?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Tags
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<List<Tag>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => await _context.Tags
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

    public async Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken ct = default)
        => await _context.Tags
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(ct);

    public async Task AddAsync(Tag tag, CancellationToken ct = default)
        => await _context.Tags.AddAsync(tag, ct);

    public Task UpdateAsync(Tag tag, CancellationToken ct = default)
    {
        _context.Tags.Update(tag);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tag tag, CancellationToken ct = default)
    {
        _context.Tags.Remove(tag);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, int userId, CancellationToken ct = default)
        => await _context.Tags.AnyAsync(t => t.Id == id && t.UserId == userId, ct);
}