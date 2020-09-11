using ApplicationCore.Entities;
using System;
using System.Security.Claims;

namespace Presentation.Interfaces
{
    public interface ITokenBuilder
    {
        string BuildToken(User user, DateTime expiresAtUtc, Claim[] claims);
    }
}
