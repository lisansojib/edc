using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class EventBindingModel : BaseViewModel
    {
        public EventBindingModel()
        {
            Speakers = new List<Select2Option>();
            Sponsors = new List<Select2Option>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public List<Select2Option> Speakers { get; set; }
        public List<Select2Option> Sponsors { get; set; }
    }
}
