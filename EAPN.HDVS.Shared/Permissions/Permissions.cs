using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Shared.Permissions
{
    public static class Permissions
    {
        //App
        public const string APP_SUPERADMIN = "user:superadmin";

        // User management
        public const string USERMANAGEMENT_ACCESS = "usermng:access";
        public const string USERMANAGEMENT_READ = "usermng:read";
        public const string USERMANAGEMENT_WRITE = "usermng:write";
        public const string USERMANAGEMENT_DELETE = "usermng:delete";

        // Personal Cards
        public const string PERSONALCARD_ACCESS = "card:access";
        public const string PERSONALCARD_READ = "card:read";
        public const string PERSONALCARD_WRITE = "card:write";
        public const string PERSONALCARD_DELETE = "card:delete";
    }
}
