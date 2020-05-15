using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAPN.HDVS.Application.Services.User
{
    public class UsuarioService : CrudServiceBase<Usuario>, IUsuarioService
    {
        private ITokenService _tokenService;
        private IPasswordService _passwordService;
        private ILoginConfiguration _loginConfiguration;

        public UsuarioService(IRepository<Usuario> repository, ILogger<UsuarioService> logger, ILoginConfiguration loginConfiguration, ITokenService tokenService, IPasswordService passwordService) : base(repository, logger)
        {
            _tokenService = tokenService;
            _passwordService = passwordService;
            _loginConfiguration = loginConfiguration;
        }

        /// <inheritdoc />
        public async Task<UserToken> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var query = GetFullUsuarioQuery();
            var usuario = await query.FirstOrDefaultAsync(x => x.Email.ToLower() == username.ToLower());
            UserToken token = null;

            if (usuario == null)
            {
                Logger.LogWarning($"Intento de login fallido. No se encontró el usuario: {username}");
                return token;
            }

            if (!usuario.Activo)
            {
                Logger.LogWarning($"Intento de login fallido. El usuario {username} intentó iniciar sesión pero no se encuentra activo");
                return token;
            }

            if (!usuario.Asociacion?.Activa ?? false)
            {
                Logger.LogWarning($"Intento de login fallido. El usuario {username} intentó iniciar sesión pero la asociación no se encuentra activa");
                return token;
            }

            if (usuario.FinBloqueo.HasValue && usuario.FinBloqueo > DateTime.UtcNow)
            {
                Logger.LogWarning($"Intento de login fallido. El usuario {username} intentó iniciar sesión pero no se encuentra bloqueado");
                return token;
            }
            else if (usuario.FinBloqueo.HasValue)
            {
                usuario.FinBloqueo = null;
                usuario.IntentosLogin = 0;
            }

            if (!_passwordService.ValidatePassword(password, usuario.Hash))
            {
                Logger.LogWarning($"Intento de login fallido. Contraseña no válida para el usuario {username}");

                usuario.IntentosLogin++;
                if (usuario.IntentosLogin > _loginConfiguration.MaxLoginAttemp)
                    usuario.FinBloqueo = DateTime.UtcNow.Add(_loginConfiguration.BlockTime);
            }
            else
            {
                Logger.LogInformation($"El usuario {username} ha iniciado sesión");
                usuario.IntentosLogin = 0;
                usuario.UltimoAcceso = DateTime.UtcNow;

                token = _tokenService.GenerateTokenForUser(usuario);
                var lifeMinutes = _tokenService.GetRefreshTokenLifeMinutes();
                usuario.Tokens = new List<RefreshToken>(new[] { new RefreshToken { FinValidez = DateTime.UtcNow.AddMinutes(lifeMinutes), Token = token.RefreshToken } });
            }

            Update(usuario);
            await SaveChangesAsync();

            return token;
        }

        /// <inheritdoc />
        public async Task<UserToken> RefreshTokenAsync(string refreshToken, int userId)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

            var query = GetFullUsuarioQuery();
            var usuario = await query.FirstOrDefaultAsync(x => x.Id == userId);
            if (usuario == null) return null;

            // Validate that user still active
            if (!usuario.Activo) return null;

            if (!usuario.Tokens.Any(x => x.Token == refreshToken && x.FinValidez >= DateTime.UtcNow)) return null;

            // Generate new token
            var token = _tokenService.GenerateTokenForUser(usuario);
            var lifeMinutes = _tokenService.GetRefreshTokenLifeMinutes();
            usuario.Tokens = new List<RefreshToken>(new[] { new RefreshToken { FinValidez = DateTime.UtcNow.AddMinutes(lifeMinutes), Token = token.RefreshToken } });

            Update(usuario);
            await SaveChangesAsync();

            return token;
        }

        /// <inheritdoc />
        public async Task LogoutAsync(ClaimsPrincipal user)
        {
            if (user == null) return;

            var id = user.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (!int.TryParse(id, out int intId)) return;

            var usuario = await Repository.GetFirstOrDefault(x => x.Id == intId, q => q.Include(x => x.Tokens));
            if (usuario == null) return;

            // Remove user refresh tokens
            usuario.Tokens = new RefreshToken[] { };

            Update(usuario);
            await SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> UpdateUserData(Usuario usuario, string currentPassword, string newPassword)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            if (!string.IsNullOrWhiteSpace(newPassword) && !_passwordService.ValidatePassword(currentPassword, usuario.Hash))
                return false;

            usuario.Hash = newPassword;
            UpdateWithtPass(usuario);
            await SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<Usuario> CreateAsync(Usuario usuario, string password)
        {
            if (string.IsNullOrEmpty(usuario?.Email)) throw new ArgumentNullException(nameof(usuario));

            usuario.IntentosLogin = 0;
            usuario.Hash = _passwordService.HashPassword(password);
            usuario.Id = 0;
            usuario.FinBloqueo = null;

            Add(usuario);
            await SaveChangesAsync();

            Logger.LogInformation($"Se ha creado el usuario {usuario.Email}");

            return usuario;
        }

        private IQueryable<Usuario> GetFullUsuarioQuery()
        {
            return Repository.EntitySet.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).ThenInclude(x => x.Permisos).ThenInclude(x => x.Permiso)
                    .Include(x => x.PermisosAdicionales).ThenInclude(x => x.Permiso)
                    .Include(x => x.Asociacion)
                    .Include(x => x.Tokens);
        }

        public void UpdateWithtPass(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.Hash))
                usuario.Hash = _passwordService.HashPassword(usuario.Hash);

            base.Update(usuario);
        }

        public override void Update(Usuario item)
        {
            item.Hash = string.Empty;
            base.Update(item);
        }

        public void UpdateRangeWithtPass(IEnumerable<Usuario> usuarios)
        {
            foreach (var usuario in usuarios)
            {
                if (!string.IsNullOrWhiteSpace(usuario.Hash))
                    usuario.Hash = _passwordService.HashPassword(usuario.Hash);
            }

            base.UpdateRange(usuarios);
        }

        public override void UpdateRange(IEnumerable<Usuario> items)
        {
            foreach (var item in items)
                item.Hash = string.Empty;

            base.UpdateRange(items);
        }
    }
}
