using System.Collections.Generic;

namespace Presentation.Participant.Models
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            Participants = new List<TeamParticipantViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamParticipantViewModel> Participants { get; set; }
    }

    public class TeamParticipantViewModel
    {
        public int Id { get; set; }
        public int TeamMemberId { get; set; }
        public string ParticipantName { get; set; }
        public string PhotoUrl { get; set; }
        public bool Disable { get; set; }
    }
}
