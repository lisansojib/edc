using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
    {
        public void Configure(EntityTypeBuilder<Speaker> builder)
        {
            builder.ToTable("Speakers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(100);

            builder.HasAlternateKey(t => t.Name).HasName("UniqueKey_SpeakerName");

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.Speakers)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_Speaker_Event")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
