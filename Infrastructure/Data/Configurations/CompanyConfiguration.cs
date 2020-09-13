using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Address).HasMaxLength(500);

            builder.Property(t => t.Phone).HasMaxLength(20);

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
