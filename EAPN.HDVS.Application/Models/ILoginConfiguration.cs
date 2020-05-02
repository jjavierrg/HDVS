using System;

namespace EAPN.HDVS.Application.Models
{
    public interface ILoginConfiguration
    {
        int MaxLoginAttemp { get; set; }
        TimeSpan BlockTime { get; set; }
    }
}
