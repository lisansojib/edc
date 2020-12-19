using System.Collections.Generic;

namespace ApplicationCore.DTOs
{
    public class PollDTO : BaseDTO
    {
        public PollDTO()
        {
            DataPoints = new List<PollDataPointDTO>();
        }

        public string Name { get; set; }
        public int GraphTypeId { get; set; }
        public int EventId { get; set; }
        public int CohortId { get; set; }
        public string GraphType { get; set; }
        public string Event { get; set; }
        public string Cohort { get; set; }
        public List<PollDataPointDTO> DataPoints { get; set; }
        public IEnumerable<Select2Option> CohortList { get; set; }
        public IEnumerable<Select2Option> GraphTypeList { get; set; }
        public IEnumerable<Select2Option> EventList { get; set; }
    }

    public class PollDataPointDTO : BaseDTO
    {
        public int PollId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
