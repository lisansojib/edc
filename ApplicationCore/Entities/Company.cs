using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Company : BaseEntity, IAuditFields
    {
        public Company()
        {
            CreatedAt = DateTime.Now;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
