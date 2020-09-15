using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models.Home
{
    public class EventViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public IEnumerable<string> Speakers { get; set; }
        public IEnumerable<string> Sponsors { get; set; }
        public IEnumerable<Select2Option> SpeakersList { get; set; }
        public IEnumerable<Select2Option> SponsorsList { get; set; }
    }
}
