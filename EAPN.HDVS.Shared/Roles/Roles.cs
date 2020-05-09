using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Shared.Roles
{
    public static class Roles
    {
        //App
        public const string APP_USERLOGIN = "user:login";

        // User management
        public const string USERMANAGEMENT_ACCESS = "usermng:access";
        public const string USERMANAGEMENT_READ = "usermng:read";
        public const string USERMANAGEMENT_WRITE = "usermng:write";
        public const string USERMANAGEMENT_DELETE = "usermng:delete";
        public const string USERMANAGEMENT_ADMIN = "usermng:admin";
    }
}
