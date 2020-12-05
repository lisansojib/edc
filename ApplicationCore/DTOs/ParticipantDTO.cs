using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class ParticipantDTO : BaseDTO
    {
        public ParticipantDTO()
        {
            CompanyList = new List<Select2Option>();
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public string PhotoUrl { get; set; }
        public string EmailCorp { get; set; }
        public string EmailPersonal { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public string CompanyName { get; set; }

        public List<Select2Option> CompanyList { get; set; }
    }
}
