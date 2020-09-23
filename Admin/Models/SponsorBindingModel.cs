using Microsoft.AspNetCore.Http;

namespace Presentation.Admin.Models
{
    public class SponsorBindingModel : BaseViewModel
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public IFormFile Logo { get; set; }
        public string Website { get; set; }
    }
}
