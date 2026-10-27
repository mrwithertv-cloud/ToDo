using Microsoft.EntityFrameworkCore;
using todolistnew.Data.Entities;

namespace todolistnew.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>()
            .HasMany(t => t.ChildToDos)
            .WithOne(t => t.Parent)
            .HasForeignKey(t => t.ParentId);
        
        modelBuilder.Entity<ToDo>()
            .HasOne(t => t.User)
            .WithMany(u => u.Todos)
            .HasForeignKey(t => t.UserId);
    }
}