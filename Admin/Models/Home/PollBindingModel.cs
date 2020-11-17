using Microsoft.AspNetCore.Http;
using System;

namespace Presentation.Admin.Models
{
    public class PollBindingModel : BaseViewModel
    {
        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int GraphTypeId { get; set; }
        public int PanelId { get; set; }
        public int OriginId { get; set; }
    }
}
