using System;

namespace Presentation.Participant.Models
{
    public class TokenBindingModel
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAtUtc { get; set; }
    }

    public class LoginBindingModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ExternalLoginBindingModel
    {
        /// <summary>
        /// Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Provider Name
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// We are storing UserId here for now.
        /// </summary>
        public string ProviderKey { get; set; }
        /// <summary>
        /// User First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
