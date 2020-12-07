using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region User
        public DbSet<Participant> ParticipantSet { get; set; }
        public DbSet<Admin> AdminSet { get; set; }
        public DbSet<ExternalLogin> ExternalLoginSet { get; set; }
        public DbSet<Guest> GuestSet { get; set; }
        #endregion

        public DbSet<Announcement> AnnouncementSet { get; set; }
        public DbSet<PollDataPoint> DataPointSet { get; set; }
        public DbSet<Event> EventSet { get; set; }
        public DbSet<ParticipantTeam> ParticipantTeamSet { get; set; }
        public DbSet<Poll> PollSet { get; set; }
        public DbSet<Speaker> SpeakerSet { get; set; }
        public DbSet<Sponsor> SponsorSet { get; set; }
        public DbSet<EventResource> EventResourceSet { get; set; }
        public DbSet<PendingSpeaker> PendingSpeakersSet { get; set; }

        public DbSet<Company> CompanySet { get; set; }
        public DbSet<Team> TeamSet { get; set; }
        public DbSet<ValueField> ValueFieldSet { get; set; }
        public DbSet<ValueFieldType> ValueFieldTypeSet { get; set; }

        public DbSet<ZoomMeeting> ZoomMeetingSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This line of code scans a given assembly for all types that implement IEntityTypeConfiguration, and registers each one automatically 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
