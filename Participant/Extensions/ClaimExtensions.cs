using ApplicationCore;
using System.Collections.Generic;
using System.Linq;

namespace System.Security.Claims
{
    public static class ClaimExtensions
    {
        public static int UserId(this IEnumerable<Claim> claims)
        {
            int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }

        public static string Username(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        }

        public static string UserRole(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }

        public static string FirstName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.FirstName).Value;
        }

        public static string FullName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.FullName).Value;
        }

        public static bool IsPersistent(this IEnumerable<Claim> claims)
        {
            bool.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.IsPersistent).Value, out bool isPersistent);
            return isPersistent;
        }

        public static bool IsGuest(this IEnumerable<Claim> claims)
        {
            bool.TryParse(claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.IsGuest).Value, out bool isGuest);
            return isGuest;
        }

        public static string UUId(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.UUId).Value;
        }
    }
}
