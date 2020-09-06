namespace Presentation.Models
{
    public class ChangePasswordBindingModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
