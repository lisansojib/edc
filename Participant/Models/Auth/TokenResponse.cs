using System;

namespace Presentation.Participant.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
