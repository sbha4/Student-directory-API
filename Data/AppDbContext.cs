using Microsoft.EntityFrameworkCore;
using Student_directory_API.Models;

namespace Student_directory_API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enforce Unique Constraints for your assignment
        modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(s => s.PhoneNumber).IsUnique();
    }
}