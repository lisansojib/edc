namespace ApplicationCore.Entities
{
    public class EventSpeaker : BaseEntity
    {
        public int EventId { get; set; }
        public int SpeakerId { get; set; }

        public virtual Event Event { get; private set; }
        public virtual Speaker Speaker { get; private set; }
    }
}
