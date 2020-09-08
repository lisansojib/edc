using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcements");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Description).HasMaxLength(1000);

            builder.Property(t => t.CallAction).IsRequired().HasMaxLength(100);

            builder.Property(t => t.LinkUrl).IsRequired().HasMaxLength(500);

            builder.Property(t => t.ImageUrl).IsRequired().HasMaxLength(250);

            builder.Property(t => t.Expiration).HasColumnType("datetime");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");        
        }
    }
}
