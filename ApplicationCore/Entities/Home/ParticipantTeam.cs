using System;

namespace ApplicationCore.Entities
{
    public class ParticipantTeam : BaseEntity, IAuditFields
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }

        /// <summary>
        /// Only Management User can create
        /// </summary>
        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Team Team { get; set; }
        public virtual Participant Participant { get; set; } 
    }
}
