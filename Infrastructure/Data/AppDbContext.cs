﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;user=root;password=Aa12345^;database=edcdb;");
        }

        #region Home
        public DbSet<Announcement> AnnouncementSet { get; set; }
        public DbSet<DataPoint> DataPointSet { get; set; }
        public DbSet<Event> EventSet { get; set; }
        public DbSet<ParticipantTeam> ParticipantTeamSet { get; set; }
        public DbSet<Poll> PollSet { get; set; }
        public DbSet<Speaker> SpeakerSet { get; set; }
        public DbSet<Sponsor> SponsorSet { get; set; }
        #endregion

        #region User
        public DbSet<Participant> ParticipantSet { get; set; }
        public DbSet<Admin> AdminSet { get; set; }
        public DbSet<ExternalLogin> ExternalLoginSet { get; set; }
        #endregion

        public DbSet<Company> CompanySet { get; set; }
        public DbSet<Team> TeamSet { get; set; }
        public DbSet<ValueField> ValueFieldSet { get; set; }
        public DbSet<ValueFieldType> ValueFieldTypeSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This line of code scans a given assembly for all types that implement IEntityTypeConfiguration, and registers each one automatically 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
