using System;

namespace Presentation.Admin.Models
{
    public class EventBindingModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
