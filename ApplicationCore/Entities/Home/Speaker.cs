namespace ApplicationCore.Entities
{
    public class Speaker : BaseEntity
    {
        public int EventId { get; set; }
        public int SpeakerId { get; set; }

        public virtual Event Event { get; private set; }
        public virtual ValueField ValueField { get; private set; }
    }
}
