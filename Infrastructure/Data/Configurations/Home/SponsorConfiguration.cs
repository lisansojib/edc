using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SponsorConfiguration : IEntityTypeConfiguration<Sponsor>
    {
        public void Configure(EntityTypeBuilder<Sponsor> builder)
        {
            builder.ToTable("Sponsors");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(100);

            builder.HasAlternateKey(t => t.Name).HasName("UniqueKey_SponsorName");

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.Sponsors)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_Sponsor_Event")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
