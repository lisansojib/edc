using System;

namespace ApplicationCore.DTOs
{
    public class EventDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
