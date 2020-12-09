using System;

namespace ApplicationCore.Entities
{
    public class ZoomMeeting : BaseEntity, IAuditFields
    {        
        public virtual new long Id { get; set; }
        public string UUId { get; set; }
        public string HostId { get; set; }
        public string Topic { get; set; }
        public string Agenda { get; set; }
        public ZoomMeetingType Type { get; set; }
        public DateTime StartTime { get; set; }
        public long Duration { get; set; }
        public string Timezone { get; set; }
        public Uri JoinUrl { get; set; }
        public string PMI { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
