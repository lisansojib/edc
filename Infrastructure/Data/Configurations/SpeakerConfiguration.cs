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

            builder.Property(t => t.FirstName).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.LastName).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.Title).HasColumnType("varchar(100)");

            builder.Property(t => t.Email).IsRequired().HasColumnType("varchar(500)");

            builder.Property(t => t.Phone).IsRequired().HasColumnType("varchar(20)");

            builder.Property(t => t.LinkedInUrl).IsRequired().HasColumnType("varchar(500)");

            builder.Property(t => t.CompanyName).HasColumnType("varchar(100)");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("getdate()");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
