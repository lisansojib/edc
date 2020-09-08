namespace ApplicationCore.Entities
{
    public class Speaker : BaseEntity
    {
        public int EventId { get; set; }
        public string Name { get; set; }

        public virtual Event Event { get; private set; }
    }
}
