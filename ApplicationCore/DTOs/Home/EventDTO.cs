using System;
using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class EventDTO : BaseDTO
    {
        public EventDTO()
        {
            EventDate = DateTime.Now;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Speakers { get; set; }
        public string Sponsors { get; set; }
        public IEnumerable<Select2Option> SpeakersList { get; set; }
        public IEnumerable<Select2Option> SponsorsList { get; set; }
        public IEnumerable<Select2Option> EventTypeList { get; set; }
        public IEnumerable<Select2Option> PresenterList { get; set; }
        public IEnumerable<Select2Option> CTOList { get; set; }
    }
}
