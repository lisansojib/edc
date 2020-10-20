using System;

namespace ApplicationCore.Entities
{
    public class PendingSpeaker : BaseEntity, IAuditFields
    {
        public PendingSpeaker()
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
        public bool IsAccepted { get; set; }
        public int AcceptedBy { get; set; }
        public DateTime? AcceptDate { get; set; }
        public bool IsRejected { get; set; }
        public int RejectedBy { get; set; }
        public int SpeakerId { get; set; }
        public DateTime? RejectDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
