using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Sponsor : BaseEntity, IAuditFields
    {
        public Sponsor()
        {
            CreatedAt = DateTime.Now;
            EventSponsors = new List<EventSponsor>();
        }

        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<EventSponsor> EventSponsors { get; set; }
    }
}
