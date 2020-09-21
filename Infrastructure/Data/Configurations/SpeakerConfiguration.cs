using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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

            builder.Property(t => t.Title).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("getdate()");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
