﻿namespace Presentation.Admin.Models
{
    public class SpeakerBindingModel : BaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int CompanyId { get; set; }
    }
}