using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CohortConfiguration : IEntityTypeConfiguration<Cohort>
    {
        public void Configure(EntityTypeBuilder<Cohort> builder)
        {
            builder.ToTable("Cohorts");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).IsRequired().HasColumnType("nvarchar(100)");

            builder.Property(t => t.Description).HasColumnType("nvarchar(500)");
        }
    }
}
