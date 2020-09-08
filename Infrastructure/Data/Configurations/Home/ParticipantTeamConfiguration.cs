using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ParticipantTeamConfiguration : IEntityTypeConfiguration<ParticipantTeam>
    {
        public void Configure(EntityTypeBuilder<ParticipantTeam> builder)
        {
            builder.ToTable("ParticipantTeams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.CreatedAt).HasColumnType("datetime");

            builder.Property(t => t.UpdatedAt).HasColumnType("datetime");

            builder
                .HasOne(t => t.Team)
                .WithMany(t => t.ParticipantTeams)
                .HasForeignKey(t => t.TeamId)
                .HasConstraintName("FK_ParticipantTeam_Team")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(t => t.Participant)
                .WithMany(t => t.ParticipantTeams)
                .HasForeignKey(t => t.TeamMemberId)
                .HasConstraintName("FK_ParticipantTeam_Participant")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
