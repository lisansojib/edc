using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Poll : BaseEntity, IAuditFields
    {
        public Poll()
        {
            DataPoints = new List<DataPoint>();
        }

        public string GraphType { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Panel { get; set; }
        public string Origin { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<DataPoint> DataPoints { get; set; }
    }
}
