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

    public class TeamMemberDTO
    {
        public string UUId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class MyTeamMemberDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string CompanyName { get; set; }
    }
}
