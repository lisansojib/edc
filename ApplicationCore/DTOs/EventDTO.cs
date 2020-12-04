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

        public int CohortId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string ImagePath { get; set; }
        public int EventTypeId { get; set; }
        public int PresenterId { get; set; }
        public int CTOId { get; set; }
        public string EventFolder { get; set; }
        public string SessionId { get; set; }
        public string Speakers { get; set; }
        public string Sponsors { get; set; }
        public string Attendees { get; set; }
        public string CohortName { get; set; }
        public IEnumerable<Select2Option> SpeakerList { get; set; }
        public IEnumerable<Select2Option> SponsorList { get; set; }
        public IEnumerable<Select2Option> EventTypeList { get; set; }
        public IEnumerable<Select2Option> PresenterList { get; set; }
        public IEnumerable<Select2Option> CTOList { get; set; }
        public IEnumerable<Select2Option> SessionList { get; set; }
        public IEnumerable<Select2Option> CohortList { get; set; }
    }

    public class SessionEventDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string ImagePath { get; set; }
        public string EventType { get; set; }
        public string CTO { get; set; }
        public string Speakers { get; set; }
        public string Sponsors { get; set; }
        public IEnumerable<EventResourceDTO> Resources { get; set; }
    }

    public class EventResourceDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string PreviewType { get; set; }
        public int EventId { get; set; }
    }
}
