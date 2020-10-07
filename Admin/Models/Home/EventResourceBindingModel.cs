using Microsoft.AspNetCore.Http;

namespace Presentation.Admin.Models.Home
{
    public class EventResourceBindingModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
