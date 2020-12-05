using Presentation.Participant.Models;
using System;

namespace Presentation.Participant.Interfaces
{
    public interface ITokenBuilder
    {
        string BuildToken(UserViewModel user, DateTime expiresAtUtc, bool isPersistent);
    }
}
