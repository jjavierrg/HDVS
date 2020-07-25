using Microsoft.AspNetCore.Authorization;

namespace EAPN.HDVS.Web.Security
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionAttribute(params string[] permissions) : base()
        {
            Roles = string.Join(",", permissions);
        }
    }
}
