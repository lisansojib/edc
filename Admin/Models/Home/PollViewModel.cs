using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Models.Home
{
    public class PollViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int GraphTypeId { get; set; }
        public int PanelId { get; set; }
        public int OriginId { get; set; }

        public string GraphType { get; set; }
        public string Panel { get; set; }
        public string Origin { get; set; }
        public IEnumerable<Select2Option> OriginList { get; set; }
        public IEnumerable<Select2Option> GraphTypeList { get; set; }
        public IEnumerable<Select2Option> PanelList { get; set; }
    }
}
