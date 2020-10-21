using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class UserDTO: BaseDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public string PhotoUrl { get; set; }
    }
}
