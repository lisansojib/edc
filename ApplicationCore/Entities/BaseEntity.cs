namespace ApplicationCore.Entities
{
    /// <summary>
    /// All Entities will inherit this class to use EfRepository
    /// 
    /// If the commission value is 0, you should keep it as 0 ,Not NULL.
    /// Why do you want to call the ISNULL function to convert it back to 0 again ?
    /// that is wrong and it makes the future programmer who is going to handle your code , sit and think 2 days why it was designed so.
    /// For more https://stackoverflow.com/a/11375589/6333717
    /// </summary>
    public abstract class BaseEntity 
    {
        public virtual int Id { get; set; }
    }
}
