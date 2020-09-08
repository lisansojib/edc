using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Management : User
    {
        public Management()
        {
            ExternalLogins = new List<ExternalLogin>();
        }

        public int AdminLevelId { get; set; }

        public virtual ValueField AdminLevel { get; set; }
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; }
    }
}
