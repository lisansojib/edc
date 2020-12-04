using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Speaker : BaseEntity, IAuditFields
    {
        public Speaker()
        {
            CreatedAt = DateTime.Now;
            EventSpeakers = new List<EventSpeaker>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
    }
}
