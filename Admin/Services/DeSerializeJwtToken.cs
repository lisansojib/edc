using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MelaMandiUI.Interfaces;

namespace MelaMandiUI.Services
{
    public class DeSerializeJwtToken : IDeSerializeJwtToken
    {
        public IEnumerable<Claim> GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(token) as JwtSecurityToken;

            return securityToken.Claims;
        }
    }
}
