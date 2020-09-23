namespace ApplicationCore.DTOs
{
    public class SponsorDTO : BaseDTO
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
    }
}
