using System;
using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class EventDTO : BaseDTO
    {
        public EventDTO()
        {
            EventDate = DateTime.Now;
            SpeakersList = new List<Select2Option>();
            SponsorsList = new List<Select2Option>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Speakers { get; set; }
        public string Sponsors { get; set; }
        public List<Select2Option> SpeakersList { get; set; }
        public List<Select2Option> SponsorsList { get; set; }
    }
}
