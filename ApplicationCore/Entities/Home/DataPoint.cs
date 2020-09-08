namespace ApplicationCore.Entities
{
    public class DataPoint : BaseEntity
    {
        public int PollId { get; set; }
        public string Name { get; set; }

        public virtual Poll Poll { get; set; }
    }
}
