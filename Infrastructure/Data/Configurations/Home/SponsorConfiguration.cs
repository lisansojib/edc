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

            builder
                .HasOne(t => t.Event)
                .WithMany(t => t.Sponsors)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_Sponsor_Event")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.ValueField)
                .WithMany(t => t.Sponsors)
                .HasForeignKey(t => t.SponsorId)
                .HasConstraintName("FK_Sponsor_ValueField")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
