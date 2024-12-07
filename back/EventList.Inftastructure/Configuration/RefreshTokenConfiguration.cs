using EventList.Domain.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventList.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(rt => rt.UserId);
            builder.Property(rt => rt.Token).IsRequired();
            builder.Property(rt => rt.Expiration).IsRequired();
        }
    }
}