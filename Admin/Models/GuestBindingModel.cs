namespace Presentation.Admin.Models.Home
{
    public class GuestBindingModel: BaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailPersonal { get; set; }
        public string EmailCorp { get; set; }
        public string PhonePersonal { get; set; }
        public string PhoneCorp { get; set; }
        public string LinkedinUrl { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
    }
}
