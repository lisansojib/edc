using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ZoomMeetingConfiguration : IEntityTypeConfiguration<ZoomMeeting>
    {
        public void Configure(EntityTypeBuilder<ZoomMeeting> builder)
        {
            builder.ToTable("ZoomMeetings");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.UUId).IsRequired().HasColumnType("varchar(128)");
            builder.Property(t => t.HostId).IsRequired().HasColumnType("varchar(128)");
            builder.Property(t => t.Type).IsRequired()
                .HasComment("1-Instant meeting, 2-Schedule meeting, 3-Recurring meeting with no fixed time, 4-Recurring meeting with fixed time");
            builder.Property(t => t.Timezone).IsRequired().HasColumnType("varchar(50)");
            builder.Property(t => t.JoinUrl).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Agenda).IsRequired().HasMaxLength(2000);
            builder.Property(t => t.PMI).IsRequired().HasColumnType("varchar(128)");
            builder.Property(t => t.CreatedAt).HasColumnType("datetime");
            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
