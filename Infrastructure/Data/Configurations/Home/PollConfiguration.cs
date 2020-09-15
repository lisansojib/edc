using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.ToTable("Polls");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.GraphType).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Date).HasColumnType("datetime");

            builder.Property(t => t.Panel).IsRequired().HasMaxLength(200);

            builder.Property(t => t.Origin).IsRequired().HasMaxLength(200);

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
