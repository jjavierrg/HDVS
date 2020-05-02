using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EAPN.HDVS.Web.Extensions
{
    public static class LoggerExtension
    {
        public static void LogUserAccessInformation(this ILogger logger, ClaimsPrincipal user, [CallerMemberName] string method = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var userName = user?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName))
                userName = "Anonymous";

            logger.LogInformation($"[{DateTime.Now}] User {userName} invokes {method} in file {sourceFilePath}:{sourceLineNumber}");
        }
        public static void LogUserAccessWarning(this ILogger logger, ClaimsPrincipal user, [CallerMemberName] string method = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var userName = user?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName))
                userName = "Anonymous";

            logger.LogWarning($"[{DateTime.Now}] User {userName} invokes {method} in file {sourceFilePath}:{sourceLineNumber}");
        }
    }
}
