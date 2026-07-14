using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}

//ApplyConfigurationsFromAssembly() — автоматически находит и применяет все классы IEntityTypeConfiguration<T> в 
//указанной сборке.

//base.OnModelCreating(builder) — вызывает реализацию базового класса DbContext 
//(обычно ничего не делает, но полезно для будущего).