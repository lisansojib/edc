using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PendingSepakerConfiguration : IEntityTypeConfiguration<PendingSepaker>
    {
        public void Configure(EntityTypeBuilder<PendingSepaker> builder)
        {
            builder.ToTable("PendingSepakers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Username).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.FirstName).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.LastName).IsRequired().HasColumnType("varchar(100)");

            builder.Property(t => t.Email).IsRequired().HasColumnType("varchar(500)");

            builder.Property(t => t.InterestInTopic).IsRequired().HasColumnType("nvarchar(500)");

            builder.Property(t => t.Phone).IsRequired().HasColumnType("varchar(20)");

            builder.Property(t => t.LinkedInUrl).IsRequired().HasColumnType("varchar(500)"); 

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
        }
    }
}
