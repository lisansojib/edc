using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class GuestDTO: BaseDTO
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
        public int GuestTypeId { get; set; }
        public int CreatedBy { get; set; }
        public virtual ValueField GuestType { get; set; }
    }
}
