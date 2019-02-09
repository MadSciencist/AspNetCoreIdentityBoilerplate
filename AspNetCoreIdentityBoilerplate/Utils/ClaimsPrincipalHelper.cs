using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCoreIdentityBoilerplate.Utils
{
    public class ClaimsPrincipalHelper
    {
        public static string GetClaimedUserIdentifier(ClaimsPrincipal principal) => principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        //public static bool IsUserAdmin(ClaimsPrincipal principal) => principal.Fin
    }
}
