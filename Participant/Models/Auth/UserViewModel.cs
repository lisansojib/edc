namespace Presentation.Participant.Models
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
            ZoomUserId = "";
            Role = "";
        }

        public string UUId { get; set; }
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
        public string Role { get; set; }
        public bool IsGuest { get; set; }
        public string ZoomUserId { get; set; }
    }
}
