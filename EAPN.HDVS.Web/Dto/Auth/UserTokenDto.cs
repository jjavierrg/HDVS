namespace EAPN.HDVS.Web.Dto.Auth
{
    public class UserTokenDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
