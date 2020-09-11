using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.IdentityModel.Tokens;
using Presentation.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace Presentation.Services
{
    public class TokenBuilder : ITokenBuilder
    {
        public TokenBuilder()
        {
        }

        public string BuildToken(User user, DateTime expiresAtUtc, Claim[] claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SYMMETRIC_SECURITY_KEY));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: expiresAtUtc);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
