using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EventSpeakerConfiguration : IEntityTypeConfiguration<EventSpeaker>
    {
        public void Configure(EntityTypeBuilder<EventSpeaker> builder)
        {
            builder.ToTable("EventSpeakers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.EventSpeakers)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_EventSpeaker_Event")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.Speaker)
                .WithMany(t => t.EventSpeakers)
                .HasForeignKey(t => t.SpeakerId)
                .HasConstraintName("FK_EventSpeaker_ValueField")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
