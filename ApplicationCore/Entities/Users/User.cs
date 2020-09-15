using System;

namespace ApplicationCore.Entities
{
    public abstract class User : BaseEntity
    {
        public User()
        {
            CreatedAt = DateTime.Now;
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }
        public string ActivationCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? DateSuspended { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
