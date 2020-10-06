namespace ApplicationCore.Entities
{
    public class EventResource : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string PreviewType { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
