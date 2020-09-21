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

            builder.Property(t => t.CompanyName).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.Description).HasMaxLength(1000);

            builder.Property(t => t.ContactPerson).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.ContactPersonEmail).IsRequired().HasColumnType("varchar(250)");

            builder.Property(t => t.ContactPersonPhone).IsRequired().HasColumnType("varchar(20)");

            builder.Property(t => t.LogoUrl).HasColumnType("varchar(250)");

            builder.Property(t => t.Website).HasColumnType("varchar(250)");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("getdate()");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
