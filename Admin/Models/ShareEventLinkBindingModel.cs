﻿namespace Presentation.Admin.Models
{
    public class ShareEventLinkBindingModel
    {
        public string[] GuestEmails { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
    }
}
