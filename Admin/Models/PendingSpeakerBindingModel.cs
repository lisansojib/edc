using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Models
{
    public class PendingSpeakerBindingModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string InterestInTopic { get; set; }
        public string ReferredBy { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public bool IsReferrer { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
    }
}
