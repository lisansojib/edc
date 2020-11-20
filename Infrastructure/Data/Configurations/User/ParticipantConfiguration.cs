using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participants");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.UUId).HasMaxLength(100);
            builder.Property(t => t.Username).IsRequired().HasColumnType("varchar(50)");
            builder.Property(t => t.FirstName).HasColumnType("varchar(100)");
            builder.Property(t => t.LastName).HasColumnType("varchar(100)");
            builder.Property(t => t.Email).IsRequired().HasColumnType("varchar(500)");
            builder.Property(t => t.Password).HasMaxLength(100);
            builder.Property(t => t.Phone).HasColumnType("varchar(20)");
            builder.Property(t => t.Mobile).HasColumnType("varchar(20)");
            builder.Property(t => t.Title).HasColumnType("varchar(100)");
            builder.Property(t => t.PhotoUrl).HasMaxLength(250);
            builder.Property(t => t.ActivationCode).HasMaxLength(128);
            builder.Property(t => t.CreatedAt).HasColumnType("datetime");
            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");
            builder.Property(t => t.DateSuspended).HasColumnType("datetime");

            builder.Property(t => t.EmailCorp).HasMaxLength(500);
            builder.Property(t => t.EmailPersonal).HasMaxLength(500);

            builder.Property(t => t.CompanyName).HasMaxLength(500);

            builder.Property(t => t.PhoneCorp).HasMaxLength(20);
            builder.Property(t => t.LinkedinUrl).HasMaxLength(250);

            builder.HasAlternateKey(t => t.Username).HasName("UniqueKey_Participant_Username");
            builder.HasAlternateKey(t => t.Email).HasName("UniqueKey_Participant_Email");

            builder.HasOne(t => t.Company)
                .WithMany(t => t.Participants)
                .HasForeignKey(t => t.CompanyId)
                .HasConstraintName("FK_Paticipant_Company")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
