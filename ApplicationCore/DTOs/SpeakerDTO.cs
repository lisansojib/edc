using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class SpeakerDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<Select2Option> CompanyList { get; set; }
    }
}
