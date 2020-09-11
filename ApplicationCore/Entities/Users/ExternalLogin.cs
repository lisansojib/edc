namespace ApplicationCore.Entities
{
    /// <summary>
    /// ExtrernalLogin entity class. Id is ignored for this entity.
    /// </summary>
    public class ExternalLogin : BaseEntity
    {
        /// <summary>
        /// Login provider i.e Facebook, Google
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// We are storing UserId here for now. We will update later
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Provider for User
        /// </summary>
        public int UserId { get; set; }

        public virtual Participant Participant { get; set; }

        public virtual Admin Management { get; set; }
    }
}
