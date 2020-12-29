using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class ProfileDTO : BaseDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string EmailCorp { get; set; }
        public string EmailPersonal { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public string CompanyName { get; set; }
    }
}
