using System.Collections.Generic;
using System.Security.Claims;

namespace MelaMandiUI.Interfaces
{
    public interface IDeSerializeJwtToken
    {
        public IEnumerable<Claim> GetClaims(string token);
    }
}
