using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(100);

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");

            builder.HasAlternateKey(t => t.Name).HasName("UniqueKey_TeamName");
        }
    }
}
