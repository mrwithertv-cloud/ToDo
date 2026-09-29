using Microsoft.EntityFrameworkCore;
using todolistnew.Entities;
namespace todolistnew.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoEntity> ToDos => Set<ToDoEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
}