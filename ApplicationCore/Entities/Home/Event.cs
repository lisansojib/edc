﻿using System;
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
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
        public virtual ICollection<EventSponsor> EventSponsors { get; set; }
    }
}
