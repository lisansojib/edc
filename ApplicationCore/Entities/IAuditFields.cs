using System;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Generic Audit information
    /// </summary>
    public interface IAuditFields
    {
        /// <summary>
        /// Created By
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// Created At
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated By
        /// </summary>
        int? UpdatedBy { get; set; }

        /// <summary>
        /// Updated At
        /// </summary>
        DateTime? UpdatedAt { get; set; }
    }
}
