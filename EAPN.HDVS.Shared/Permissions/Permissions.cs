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

        // Indicators
        public const string PERSONALINDICATORS_ACCESS = "indicators:access";
        public const string PERSONALINDICATORS_READ = "indicators:read";
        public const string PERSONALINDICATORS_WRITE = "indicators:write";
        public const string PERSONALINDICATORS_DELETE = "indicators:delete";

        // Indicators
        public const string PERSONALATTACHMENTS_ACCESS = "attachments:access";
        public const string PERSONALATTACHMENTS_READ = "attachments:read";
        public const string PERSONALATTACHMENTS_WRITE = "attachments:write";
        public const string PERSONALATTACHMENTS_DELETE = "attachments:delete";

        // Indicators
        public const string STATS_ACCESS = "stats:access";
        public const string STATS_GLOBAL = "stats:global";
    }
}
