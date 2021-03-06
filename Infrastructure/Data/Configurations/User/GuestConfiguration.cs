﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guests");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Password).HasMaxLength(100);
            builder.Property(t => t.PlainPassword).HasMaxLength(20);
            builder.Property(t => t.FirstName).HasColumnType("varchar(100)");
            builder.Property(t => t.LastName).HasColumnType("varchar(100)");
            builder.Property(t => t.Email).HasMaxLength(500).IsRequired();
            builder.Property(t => t.EmailPersonal).HasMaxLength(500);
            builder.Property(t => t.EmailCorp).HasMaxLength(500);
            builder.Property(t => t.PhonePersonal).HasMaxLength(20);
            builder.Property(t => t.PhoneCorp).HasMaxLength(20);
            builder.Property(t => t.CompanyName).HasMaxLength(100);
            builder.Property(t => t.Title).HasColumnType("varchar(100)");
            builder.Property(t => t.CreatedAt).HasColumnType("datetime");
            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
            builder.Property(t => t.LinkedinUrl).HasMaxLength(250);
            builder.Property(t => t.Role).IsRequired().HasColumnType("varchar(20)").HasComment("Guest,Member,Speaker,Sponsor");
        }
    }
}
