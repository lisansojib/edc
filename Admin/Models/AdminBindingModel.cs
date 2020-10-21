using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Models
{
    public class AdminBindingModel: BaseViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public string PhotoUrl { get; set; }
    }
}
