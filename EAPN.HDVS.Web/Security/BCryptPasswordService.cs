using EAPN.HDVS.Application.Security;

namespace EAPN.HDVS.Web.Security
{
    public class BCryptPasswordService : IPasswordService
    {
        private string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public bool ValidatePassword(string password, string expectedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, expectedHash);
        }
    }
}
