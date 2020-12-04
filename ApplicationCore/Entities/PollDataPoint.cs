namespace ApplicationCore.Entities
{
    public class PollDataPoint : BaseEntity
    {
        public int PollId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public virtual Poll Poll { get; set; }
    }
}
