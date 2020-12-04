using System;

namespace ApplicationCore.Entities
{
    public class Announcement : BaseEntity, IAuditFields
    {
        public Announcement()
        {
            CreatedAt = DateTime.Now;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string CallAction { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Expiration { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
