using ApplicationCore;
using System.Collections.Generic;
using System.Linq;

namespace System.Security.Claims
{
    public static class ClaimExtensions
    {
        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }

        public static string GetUsername(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        }

        public static string GetUserRole(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }

        public static string GetFirstName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.FirstName).Value;
        }

        public static string GetFullName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.FullName).Value;
        }

        public static bool GetIsPersistent(this IEnumerable<Claim> claims)
        {
            bool.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.IsPersistent).Value, out bool isPersistent);
            return isPersistent;
        }

        public static bool GetIsGuest(this IEnumerable<Claim> claims)
        {
            bool.TryParse(claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.IsGuest).Value, out bool isGuest);
            return isGuest;
        }
    }
}
