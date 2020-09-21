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
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Company Company { get; private set; }
        public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
    }
}
