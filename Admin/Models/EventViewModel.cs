using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models.Home
{
    public class EventViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int EventTypeId { get; set; }
        public int CohortId { get; set; }
        public int CTOId { get; set; }
        public DateTime EventDate { get; set; }
        public IEnumerable<string> SpeakerIds { get; set; }
        public IEnumerable<string> SponsorIds { get; set; }
        public IEnumerable<Select2Option> SpeakerList { get; set; }
        public IEnumerable<Select2Option> SponsorList { get; set; }
        public IEnumerable<Select2Option> EventTypeList { get; set; }
        public IEnumerable<Select2Option> PresenterList { get; set; }
        public IEnumerable<Select2Option> CTOList { get; set; }
        public IEnumerable<Select2Option> SessionList { get; set; }
        public IEnumerable<Select2Option> CohortList { get; set; }
    }
}
