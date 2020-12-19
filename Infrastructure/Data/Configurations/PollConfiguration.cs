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

            builder.Property(t => t.GraphTypeId).IsRequired();

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            builder.Property(t => t.EventId).IsRequired();

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
