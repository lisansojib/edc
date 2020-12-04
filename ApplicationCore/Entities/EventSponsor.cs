namespace ApplicationCore.Entities
{
    public class EventSponsor : BaseEntity
    {
        public int EventId { get; set; }
        public int SponsorId { get; set; }

        public virtual Event Event { get; private set; }
        public virtual Sponsor Sponsor { get; private set; }
    }
}
