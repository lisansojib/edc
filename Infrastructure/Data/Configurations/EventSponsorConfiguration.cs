using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EventSponsorConfiguration : IEntityTypeConfiguration<EventSponsor>
    {
        public void Configure(EntityTypeBuilder<EventSponsor> builder)
        {
            builder.ToTable("EventSponsors");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.EventSponsors)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_EventSponsor_Event")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.Sponsor)
                .WithMany(t => t.EventSponsors)
                .HasForeignKey(t => t.SponsorId)
                .HasConstraintName("FK_EventSponsor_Sponsor")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
