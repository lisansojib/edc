using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Company : BaseEntity, IAuditFields
    {
        public Company()
        {
            Participants = new List<Participant>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}
