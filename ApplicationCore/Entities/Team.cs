using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Team : BaseEntity, IAuditFields
    {
        public Team()
        {
            ParticipantTeams = new List<ParticipantTeam>();
            CreatedAt = DateTime.Now;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ParticipantTeam> ParticipantTeams { get; set; }
    }
}
