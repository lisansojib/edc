using System;

namespace ApplicationCore.Entities
{
    public class Guest : BaseEntity, IAuditFields
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailPersonal { get; set; }
        public string EmailCorp { get; set; }
        public string PhonePersonal { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public int GeustTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ValueField GuestType { get; set; }
    }
}
