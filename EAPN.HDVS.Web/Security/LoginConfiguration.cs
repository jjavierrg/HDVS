using EAPN.HDVS.Application.Models;
using System;

namespace EAPN.HDVS.Web.Security
{
    public class LoginConfiguration: ILoginConfiguration
    {
        public int MaxLoginAttemp { get; set; }
        public TimeSpan BlockTime { get; set; }
    }
}
