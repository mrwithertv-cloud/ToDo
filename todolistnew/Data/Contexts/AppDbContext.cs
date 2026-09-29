using Microsoft.EntityFrameworkCore;
using todolistnew.Data.Entities;

namespace todolistnew.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDos => Set<ToDo>();
    public DbSet<User> Users => Set<User>();
}