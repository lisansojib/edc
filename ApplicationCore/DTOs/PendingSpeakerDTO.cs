using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class PendingSpeakerDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string InterestInTopic { get; set; }
        public string ReferredBy { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public bool IsReferrer { get; set; }
    }
}
