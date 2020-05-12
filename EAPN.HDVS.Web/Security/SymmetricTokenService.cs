using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EAPN.HDVS.Web.Security
{
    public class SymmetricTokenService : ITokenService
    {
        private ITokenConfiguration _tokenConfiguration;
        
        /// <summary>
        /// </summary>
        /// <param name="tokenConfiguration"></param>
        public SymmetricTokenService(ITokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        /// <summary>
        /// Obtiene los minutos de válidez de un token de refresco (por defecto 1 día)
        /// </summary>
        /// <returns></returns>
        public int GetRefreshTokenLifeMinutes()
        {
            return _tokenConfiguration?.RefreshTokenLifeMinutes ?? (24 * 60);
        }

        /// <summary>
        /// Genera un token de acceso válido para un usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserToken GenerateTokenForUser(Usuario user)
        {
            if (user == null)
                return null;

            var permisos = user.Perfiles.SelectMany(x => x.Perfil?.Roles).Select(x => x.Rol)
                            .Union(user.RolesAdicionales.Select(x => x.Rol))
                            .Distinct();

            var claims = new List<Claim>(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Nombre ?? ""),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Apellidos ?? ""),
                new Claim("asociacion_id", user.AsociacionId.ToString()),
            });

            var secondsExpire = Math.Max(_tokenConfiguration.TokenLifeMinutes, 10) * 60;

            if (permisos.Any())
                claims.AddRange(permisos.Select(x => new Claim(ClaimTypes.Role, x.Permiso)));
            else
                claims.Add(new Claim(ClaimTypes.Role, "Anonymous"));

            var key = Encoding.ASCII.GetBytes(_tokenConfiguration.SymmetricSecret);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(secondsExpire),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return new UserToken
            {
                AccessToken = tokenHandler.WriteToken(tokenDescriptor),
                ExpiresIn = secondsExpire,
                RefreshToken = GenerateRefreshToken()
            };
        }

        /// <summary>
        /// Genera una cadena aleatoria para ser utilizada como token de refresco
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            int size = 64;
            var randomNumber = new byte[size];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
