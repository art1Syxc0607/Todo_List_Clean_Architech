using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
        => _context = context;

    public async Task<UserTask?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<List<UserTask>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => await _context.Tasks
            .Where(t => t.UserId == userId)
            .Include(t => t.Tags)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(UserTask task, CancellationToken ct = default)
        => await _context.Tasks.AddAsync(task, ct);

    public Task UpdateAsync(UserTask task, CancellationToken ct = default)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UserTask task, CancellationToken ct = default)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _context.Tasks.AnyAsync(t => t.Id == id, ct);
}