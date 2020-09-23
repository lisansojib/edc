using Microsoft.AspNetCore.Http;

namespace Presentation.Admin.Models
{
    public class ParticipantBindingModel : BaseViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public IFormFile Photo { get; set; }
        public string EmailCorp { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public int CompanyId { get; set; }
    }
}
