using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Participant : User
    {
        public Participant()
        {
            ParticipantTeams = new List<ParticipantTeam>();
            ExternalLogins = new List<ExternalLogin>();
        }

        public string EmailCorp { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<ParticipantTeam> ParticipantTeams { get; set; }
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; }
    }
}
