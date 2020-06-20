namespace EAPN.HDVS.Application.Models
{
    public interface ITokenConfiguration
    {
        string SymmetricSecret { get; set; }
        int TokenLifeMinutes { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
    }
}
