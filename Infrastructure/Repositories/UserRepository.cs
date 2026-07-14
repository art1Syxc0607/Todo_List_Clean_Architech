using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
        => _context = context;

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Users
            .Include(u => u.Tasks)
            .Include(u => u.Categories)
            .Include(u => u.Tags)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName, ct);

    public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
        => await _context.Users
            .OrderBy(u => u.UserName)
            .ToListAsync(ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await _context.Users.AddAsync(user, ct);

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.UserName == userName, ct);

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.Id == id, ct);
}