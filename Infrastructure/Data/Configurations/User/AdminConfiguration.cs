using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Username).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Password).HasMaxLength(100);
            builder.Property(t => t.FirstName).HasMaxLength(100);
            builder.Property(t => t.LastName).HasMaxLength(100);
            builder.Property(t => t.Phone).HasMaxLength(20);
            builder.Property(t => t.Mobile).HasMaxLength(20);
            builder.Property(t => t.Title).HasMaxLength(100);
            builder.Property(t => t.PhotoUrl).HasMaxLength(128);
            builder.Property(t => t.ActivationCode).HasMaxLength(128);
            builder.Property(t => t.CreatedAt).HasColumnType("datetime");
            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
            builder.Property(t => t.DateSuspended).HasColumnType("datetime");

            builder.HasAlternateKey(t => t.Username).HasName("UniqueKey_Admin_Username");
            builder.HasAlternateKey(t => t.Email).HasName("UniqueKey_Admin_Email");

            builder.HasOne(t => t.AdminLevel)
                .WithMany(t => t.Managements)
                .HasForeignKey(t => t.AdminLevelId)
                .HasConstraintName("FK_Management_AdminLevel")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
