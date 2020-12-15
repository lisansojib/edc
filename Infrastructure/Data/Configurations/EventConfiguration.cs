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

            builder.Property(t => t.ImagePath).HasMaxLength(250).HasDefaultValue("");

            builder.Property(t => t.EventFolder).HasColumnType("varchar(50)");

            builder.Property(t => t.SessionId).HasMaxLength(100);

            builder.Property(t => t.Description).HasMaxLength(1000);

            builder.Property(t => t.EventDate).HasColumnType("datetime");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");

            builder.Property(t => t.MeetingPassword).HasColumnType("varchar(20)");

            builder
                .HasOne(t => t.Cohort)
                .WithMany(t => t.Events)
                .HasForeignKey(t => t.CohortId)
                .HasConstraintName("FK_Event_Cohort")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
