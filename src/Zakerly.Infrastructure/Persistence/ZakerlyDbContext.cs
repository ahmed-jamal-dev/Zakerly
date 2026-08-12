using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;

namespace Zakerly.Infrastructure.Persistence;

public class ZakerlyDbContext : DbContext
{
    public ZakerlyDbContext(DbContextOptions<ZakerlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<Assignment> Assignments => Set<Assignment>();

    public DbSet<Submission> Submissions => Set<Submission>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZakerlyDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}