﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
    {
        public void Configure(EntityTypeBuilder<ExternalLogin> builder)
        {
            builder.ToTable("ExternalLogins");

            builder.HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });

            builder.Ignore(x => x.Id);

            builder.Property(x => x.LoginProvider).IsRequired().HasColumnType("varchar(100)"); // HasMaxLength ignored if HasColumnType set

            builder.Property(x => x.ProviderKey).IsRequired().HasColumnType("varchar(128)");

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.UserLogins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserLogins_User");
        }
    }
}
