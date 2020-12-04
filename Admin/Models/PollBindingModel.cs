using System;
using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class PollBindingModel : BaseViewModel
    {
        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int GraphTypeId { get; set; }
        public int PanelId { get; set; }
        public int OriginId { get; set; }

        public IEnumerable<PollDataPointBindingModel> DataPoints { get; set; }
    }

    public class PollDataPointBindingModel : BaseViewModel
    {
        public int PollId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
