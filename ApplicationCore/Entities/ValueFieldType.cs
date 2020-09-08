using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// This Entity is used for various types of Setup Type.
    /// For Example 'Ad Promotion Type'
    /// </summary>
    public class ValueFieldType : BaseEntity
    {
        public ValueFieldType()
        {
            Description = "";
            ValueFields = new List<ValueField>();
        }

        /// <summary>
        /// Selection Item Type Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Selection Item Type description
        /// </summary>
        public string Description { get; set; }

        public virtual ICollection<ValueField> ValueFields { get; set; }
    }
}
