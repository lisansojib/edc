namespace ApplicationCore.DTOs
{
    public class MyTeamDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ParticipantTeamId { get; set; }
        public int TeamMemberId { get; set; }
        public string ParticipantName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
