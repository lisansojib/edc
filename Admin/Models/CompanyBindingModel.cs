using Microsoft.AspNetCore.Http;

namespace Presentation.Admin.Models
{
    public class CompanyBindingModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IFormFile Logo { get; set; }
        public string Website { get; set; }
    }
}
