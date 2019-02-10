using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AspNetCoreIdentityBoilerplate.Utils
{
    public class ClaimsPrincipalHelper
    {
        public static bool HasUserClaimedIdentifier(ClaimsPrincipal principal, string claimedId)
        {
            return GetClaimedIdentifier(principal).Equals(claimedId);
        }

        public static string GetClaimedIdentifier(ClaimsPrincipal principal) => principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        public static bool IsUserAdmin(ClaimsPrincipal principal) => GetClaimedRoles(principal).Contains("admin");

        public static IEnumerable<string> GetClaimedRoles(ClaimsPrincipal principal) =>
            principal.FindAll(ClaimTypes.Role).Select(x => x.Value);
    }
}
