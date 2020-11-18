namespace Presentation.Participant.Models
{
    public class PendingSpeakerBindingModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InterestInTopic { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public bool IsReferrer { get; set; }
        public int PanelId { get; set; }
    }
}
