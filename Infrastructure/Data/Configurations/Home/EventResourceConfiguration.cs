using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Home
{
    public class EventResourceConfiguration : IEntityTypeConfiguration<EventResource>
    {
        public void Configure(EntityTypeBuilder<EventResource> builder)
        {
            builder.ToTable("EventResources");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Title).IsRequired().HasColumnType("nvarchar(100)");

            builder.Property(t => t.Description).HasColumnType("nvarchar(200)");

            builder.Property(t => t.FilePath).IsRequired().HasColumnType("varchar(250)");

            builder.Property(t => t.PreviewType).IsRequired().HasColumnType("varchar(20)");

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.EventResources)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_EventResource_Event")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
