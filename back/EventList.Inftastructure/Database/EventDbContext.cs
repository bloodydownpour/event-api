using Microsoft.EntityFrameworkCore;
using EventList.Domain.Data;
namespace EventList.Infrastructure.Database;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EventUser> EventUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);
    }
}
