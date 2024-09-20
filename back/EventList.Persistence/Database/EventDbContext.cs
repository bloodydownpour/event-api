using Microsoft.EntityFrameworkCore;
using EventList.Domain.Data;
namespace EventList.Persistence.Database
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EventUser>()
           .HasKey(eu => new { eu.EventId, eu.UserId });

            modelBuilder.Entity<EventUser>()
            .HasOne<Event>()
            .WithMany()
            .HasForeignKey(eu => eu.EventId);

            modelBuilder.Entity<EventUser>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(eu => eu.UserId);
        }
    }
}
