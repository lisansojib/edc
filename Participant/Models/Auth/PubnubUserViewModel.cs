using ApplicationCore.DTOs;
using System.Collections.Generic;

namespace Presentation.Participant.Models
{
    public class PubnubUserViewModel : BaseViewModel
    {
        public string UUId { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<ChannelDTO> Channels { get; set; }
        public IEnumerable<TeamMemberDTO> TeamMembers { get; set; }
        public IEnumerable<SessionEventDTO> Events { get; set; }
    }
}
