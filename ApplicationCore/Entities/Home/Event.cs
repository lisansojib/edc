using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Event : BaseEntity, IAuditFields
    {
        public Event()
        {
            CreatedAt = DateTime.Now;
            EventSpeakers = new List<EventSpeaker>();
            EventSponsors = new List<EventSponsor>();
            EventResources = new List<EventResource>();
        }

        public int CohortId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string ImagePath { get; set; }
        public int EventTypeId { get; set; }
        public int PresenterId { get; set; }
        public int CTOId { get; set; }
        public string EventFolder { get; set; }
        public string SessionId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }

        /// <summary>
        /// Upto 5 sponsors
        /// </summary>
        public virtual ICollection<EventSponsor> EventSponsors { get; set; }

        public virtual List<EventResource> EventResources { get; set; }

        public virtual Cohort Cohort { get; set; }
    }
}
