using System.Collections.Generic;

namespace Presentation.Participant.Models
{
    public class ProfileViewModel : BaseViewModel
    {
        public string UUId { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<string> Channels { get; set; }
    }
}
