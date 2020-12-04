using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Participant : User
    {
        public Participant()
        {
            Active = true;
            ParticipantTeams = new List<ParticipantTeam>();
            ExternalLogins = new List<ExternalLogin>();
        }

        public string EmailPersonal { get; set; }
        public string EmailCorp { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public virtual ICollection<ParticipantTeam> ParticipantTeams { get; set; }
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; }
    }
}
