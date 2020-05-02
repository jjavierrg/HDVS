using EAPN.HDVS.Entities;

namespace EAPN.HDVS.Application.Security
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string expectedHash);
    }
}
