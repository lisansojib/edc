using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class EventBindingModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Cohort { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int EventTypeId { get; set; }
        public int PresenterId { get; set; }
        public int CTOId { get; set; }
        public IEnumerable<Select2Option> Speakers { get; set; }
        public IEnumerable<Select2Option> Sponsors { get; set; }
    }
}
