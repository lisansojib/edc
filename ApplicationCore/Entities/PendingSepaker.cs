using System;

namespace ApplicationCore.Entities
{
    public class PendingSepaker : BaseEntity, IAuditFields
    {
        public PendingSepaker()
        {
            CreatedAt = DateTime.Now;
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string InterestInTopic { get; set; }
        public int ReferredBy { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public bool IsReferrer { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
