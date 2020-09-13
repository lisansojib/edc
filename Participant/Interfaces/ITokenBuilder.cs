using ApplicationCore.Entities;
using System;
using System.Security.Claims;

namespace Presentation.Participant.Interfaces
{
    public interface ITokenBuilder
    {
        string BuildToken(User user, DateTime expiresAtUtc, bool isPersistent);
    }
}
