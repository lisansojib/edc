using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// This Entity is used for Setup Type values.
    /// For Example 'Ad Promotion Type has 'Free, Urgent, Featured' values
    /// </summary>
    public class ValueField : BaseEntity
    {
        public ValueField()
        {
            Description = "";
            Managements = new List<Admin>();
        }

        /// <summary>
        /// Selection Item Value Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of selection Item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Selection Master Id
        /// </summary>
        public int TypeId { get; set; }

        public int SeqNo { get; set; }

        public virtual ValueFieldType ValueFieldType { get; set; }

        public virtual ICollection<Admin> Managements { get; private set; }
        public virtual ICollection<Guest> Guests { get; private set; }
    }
}
