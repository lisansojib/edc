using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Cohort : BaseEntity
    {
        public Cohort()
        {
            Events = Events;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
