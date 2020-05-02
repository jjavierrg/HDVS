using EAPN.HDVS.Application.Models;

namespace EAPN.HDVS.Web.Security
{
    public class TokenConfiguration : ITokenConfiguration
    {
        public string SymmetricSecret { get; set; }
        public int TokenLifeMinutes { get; set; }
        public int RefreshTokenLifeMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
