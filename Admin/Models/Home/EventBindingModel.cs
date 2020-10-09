using ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;
using Presentation.Admin.Models.Home;
using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class EventBindingModel : BaseViewModel
    {
        public EventBindingModel()
        {
            Resources = new List<EventResourceBindingModel>();
        }

        public string Title { get; set; }
        public string Cohort { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int EventTypeId { get; set; }
        public int PresenterId { get; set; }
        public int CTOId { get; set; }
        public string SessionId { get; set; }
        public IEnumerable<Select2Option> Speakers { get; set; }
        public IEnumerable<Select2Option> Sponsors { get; set; }
        public List<EventResourceBindingModel> Resources { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
