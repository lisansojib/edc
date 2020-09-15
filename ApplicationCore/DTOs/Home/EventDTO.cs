using System;

namespace ApplicationCore.DTOs
{
    public class EventDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Speakers { get; set; }
        public string Sponsors { get; set; }
    }
}
