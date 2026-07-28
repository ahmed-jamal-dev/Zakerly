using Microsoft.EntityFrameworkCore;
using Zakerly.Domain.Entities;

namespace Zakerly.Infrastructure.Persistence;

public class ZakerlyDbContext : DbContext
{
    public ZakerlyDbContext(DbContextOptions<ZakerlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}