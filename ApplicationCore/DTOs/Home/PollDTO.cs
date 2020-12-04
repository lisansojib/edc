using System;
using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class PollDTO : BaseDTO
    {
        public PollDTO()
        {
            Name = "";
            PollDate = DateTime.Now;
            DataPoints = new List<PollDataPointDTO>();
        }

        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int GraphTypeId { get; set; }
        public int PanelId { get; set; }
        public int OriginId { get; set; }
        public string GraphType { get; set; }
        public string PanelName { get; set; }
        public string OriginName { get; set; }
        public List<PollDataPointDTO> DataPoints { get; set; }
        public IEnumerable<Select2Option> OriginList { get; set; }
        public IEnumerable<Select2Option> GraphTypeList { get; set; }
        public IEnumerable<Select2Option> PanelList { get; set; }
    }

    public class PollDataPointDTO : BaseDTO
    {
        public int PollId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
