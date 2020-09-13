using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Event : BaseEntity, IAuditFields
    {
        public Event()
        {
            CreatedAt = DateTime.Now;
            Speakers = new List<Speaker>();
            Sponsors = new List<Sponsor>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<Sponsor> Sponsors { get; set; }
    }
}
