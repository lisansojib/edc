﻿using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class PendingSpeakerDTO : BaseDTO
    {
        public PendingSpeakerDTO()
        {
            InterestInTopic = "";
            Note = "";
        }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string InterestInTopic { get; set; }
        public string Note { get; set; }
        public int PanelId { get; set; }
        public int CompanyId { get; set; }
        public string ReferredBy { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public bool IsReferrer { get; set; }
        public IEnumerable<Select2Option> PanelList { get; set; }
        public IEnumerable<Select2Option> CompanyList { get; set; }
    }
}
