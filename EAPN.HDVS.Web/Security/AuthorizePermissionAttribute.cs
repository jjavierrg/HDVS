using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
