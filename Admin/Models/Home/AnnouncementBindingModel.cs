using System;

namespace Presentation.Admin.Models
{
    public class AnnouncementBindingModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CallAction { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Expiration { get; set; }
    }
}
