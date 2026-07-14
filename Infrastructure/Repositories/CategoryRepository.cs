using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
        => _context = context;

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Categories
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<List<Category>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => await _context.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public async Task AddAsync(Category category, CancellationToken ct = default)
        => await _context.Categories.AddAsync(category, ct);

    public Task UpdateAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Update(category);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, int userId, CancellationToken ct = default)
        => await _context.Categories.AnyAsync(c => c.Id == id && c.UserId == userId, ct);
}