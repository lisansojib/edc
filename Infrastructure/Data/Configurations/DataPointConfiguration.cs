using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DataPointConfiguration : IEntityTypeConfiguration<PollDataPoint>
    {
        public void Configure(EntityTypeBuilder<PollDataPoint> builder)
        {
            builder.ToTable("PollDataPoints");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(100);

            builder.HasAlternateKey(t => t.Name).HasName("UniqueKey_DataPointName");

            builder
                .HasOne(t => t.Poll)
                .WithMany(t => t.DataPoints)
                .HasForeignKey(t => t.PollId)
                .HasConstraintName("FK_DataPoint_Poll")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
