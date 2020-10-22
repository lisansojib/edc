using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class ChannelAndTeamMembersDTO
    {
        public IEnumerable<ChannelDTO> Channels { get; set; }
        public IEnumerable<TeamMemberDTO> TeamMembers { get; set; }
        public IEnumerable<SessionEventDTO> Events { get; set; }
    }

    public class ChannelDTO 
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCohort { get; set; }
    }
}
