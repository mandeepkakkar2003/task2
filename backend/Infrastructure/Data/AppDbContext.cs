using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();
        b.Entity<Project>()
            .Property(p => p.Title).IsRequired().HasMaxLength(100);
        b.Entity<Project>()
            .Property(p => p.Description).HasMaxLength(500);
        b.Entity<TaskItem>()
            .Property(t => t.Title).IsRequired().HasMaxLength(200);

        b.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project!)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
