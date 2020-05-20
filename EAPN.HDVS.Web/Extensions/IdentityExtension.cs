using EAPN.HDVS.Shared.Permissions;
using System.Linq;
using System.Security.Claims;

namespace EAPN.HDVS.Web.Extensions
{
    public static class IdentityExtension
    {
        public static int GetUserOrganizacionId(this ClaimsPrincipal user)
        {
            if (!user.HasClaim(x => x.Type == "organizacion_id"))
                return 0;

            var claimValue = user.Claims.First(x => x.Type == "organizacion_id").Value;
            if (!int.TryParse(claimValue, out int result))
                return 0;

            return result;
        }
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            if (!user.HasClaim(x => x.Type == ClaimTypes.Name))
                return null;

            var claimValue = user.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            if (!int.TryParse(claimValue, out int result))
                return null;

            return result;
        }

        public static bool HasSuperAdminPermission(this ClaimsPrincipal user)
        {
            return user.IsInRole(Permissions.APP_SUPERADMIN);
        }
    }
}
