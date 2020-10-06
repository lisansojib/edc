using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Cohort).HasMaxLength(100).HasDefaultValue("");

            builder.Property(t => t.ImagePath).HasMaxLength(250).HasDefaultValue("");

            builder.Property(t => t.SessionId).HasMaxLength(100);

            builder.Property(t => t.Description).HasMaxLength(1000);

            builder.Property(t => t.EventDate).HasColumnType("datetime");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");  
        }
    }
}
