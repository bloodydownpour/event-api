using EventList.Domain.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
    
namespace EventList.Infrastructure.Configuration
{
    public class EventUserConfiguration : IEntityTypeConfiguration<EventUser>
    {
        public void Configure(EntityTypeBuilder<EventUser> builder)
        {
            builder.ToTable("EventUsers");
            builder.HasKey(eu => new { eu.EventId, eu.UserId });

            builder.HasOne<Event>()
                   .WithMany()
                   .HasForeignKey(eu => eu.EventId);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(eu => eu.UserId);
        }
    }
}
