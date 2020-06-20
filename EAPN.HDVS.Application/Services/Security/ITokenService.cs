using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Entities;

namespace EAPN.HDVS.Application.Security
{
    public interface ITokenService
    {
        public UserToken GenerateTokenForUser(Usuario user);
    }
}
