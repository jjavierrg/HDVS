using EAPN.HDVS.Application.Security;

namespace EAPN.HDVS.Web.Security
{
    public class BCryptPasswordService : IPasswordService
    {
        private string GetRandomSalt() => BCrypt.Net.BCrypt.GenerateSalt(12);

        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());

        public bool ValidatePassword(string password, string correctHash) => BCrypt.Net.BCrypt.Verify(password, correctHash);
    }
}
