using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.IdentityModel.Tokens;
using Presentation.Participant.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace Presentation.Participant.Services
{
    public class TokenBuilder : ITokenBuilder
    {
        public string BuildToken(User user, DateTime expiresAtUtc, bool isPersistent)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.IsPersistent, isPersistent.ToString()),
                new Claim(AdditionalClaimTypes.FirstName, user.FirstName),
                new Claim(AdditionalClaimTypes.FullName, $"{user.FirstName} {user.LastName}")
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SYMMETRIC_SECURITY_KEY));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: expiresAtUtc);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
