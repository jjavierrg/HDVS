using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Services.User
{
    public interface IUsuarioService : ICrudServiceBase<Usuario>
    {
        /// <summary>
        /// Regiter a new User in the system. Email field is mandatory
        /// </summary>
        /// <param name="username">User login</param>
        /// <param name="password">User Password</param>
        /// <returns>User Token</returns>
        Task<Usuario> CreateAsync(Usuario usuario, string password);

        /// <summary>
        /// Find a registered user, validates password and generate a token if all information is correct
        /// </summary>
        /// <param name="username">User definition</param>
        /// <param name="password">User password</param>
        /// <returns>Created User</returns>
        Task<UserToken> LoginAsync(string username, string password);
        /// <summary>
        /// Perform user Logout
        /// </summary>
        /// <param name="user">User Principal</param>
        Task LogoutAsync(ClaimsPrincipal user);

        /// <summary>
        /// Refresh user token
        /// </summary>
        /// <param name="refreshToken">refresh token associated to User</param>
        /// <param name="user">User Principal</param>
        /// <returns>New extended User Token</returns>
        Task<UserToken> RefreshTokenAsync(string refreshToken, int userId);

        /// <summary>
        /// Store user changes and update password field
        /// </summary>
        /// <param name="usuario">User data to update</param>
        void UpdateWithtPass(Usuario usuario);

        /// <summary>
        /// Store a range of user changes and update their password field
        /// </summary>
        /// <param name="usuarios">user list to update</param>
        void UpdateRangeWithtPass(IEnumerable<Usuario> usuarios);
        Task<bool> UpdateUserData(Usuario usuario, string currentPassword, string newPassword);
    }
}
