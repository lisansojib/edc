using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Poll : BaseEntity, IAuditFields
    {
        public Poll()
        {
            CreatedAt = DateTime.Now;
            DataPoints = new List<PollDataPoint>();
        }
        
        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int GraphTypeId { get; set; }
        public int PanelId { get; set; }
        public int OriginId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<PollDataPoint> DataPoints { get; set; }
    }
}
