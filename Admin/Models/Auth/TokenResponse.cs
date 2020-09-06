using System;

namespace Presentation.Admin.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
