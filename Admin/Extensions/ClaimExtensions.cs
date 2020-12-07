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

        public static bool GetIsPersistent(this IEnumerable<Claim> claims)
        {
            bool.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.IsPersistent).Value, out bool isPersistent);
            return isPersistent;
        }

        public static string GetZoomUserId(this IEnumerable<Claim> claims)
        {
            var zoomUserId =  claims.FirstOrDefault(x => x.Type == AdditionalClaimTypes.ZoomUserId).Value;
            return zoomUserId.NullOrEmpty() ? Constants.MAX_ZOOM_USER_ID : zoomUserId;
        }
    }
}
