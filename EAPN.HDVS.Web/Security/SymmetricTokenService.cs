using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EAPN.HDVS.Web.Security
{
    /// <summary>
    /// </summary>
    public class SymmetricTokenService : ITokenService
    {
        private readonly ITokenConfiguration _tokenConfiguration;

        /// <summary>
        /// </summary>
        /// <param name="tokenConfiguration"></param>
        public SymmetricTokenService(ITokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        /// <summary>
        /// Genera un token de acceso válido para un usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserToken GenerateTokenForUser(Usuario user)
        {
            if (user == null)
            {
                return null;
            }

            var permisos = user.Perfiles.SelectMany(x => x.Perfil?.Permisos).Select(x => x.Permiso)
                            .Union(user.PermisosAdicionales.Select(x => x.Permiso))
                            .Distinct();

            var claims = new List<Claim>(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Nombre ?? ""),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Apellidos ?? ""),
                new Claim(ClaimTypes.Name, user.Id.ToString()), // Used user Id as name for logging purposes
                new Claim("organizacion_id", user.OrganizacionId.ToString()),
            });

            var secondsExpire = Math.Max(_tokenConfiguration.TokenLifeMinutes, 10) * 60;

            if (permisos.Any())
            {
                claims.AddRange(permisos.Select(x => new Claim(ClaimTypes.Role, x.Clave)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Anonymous"));
            }

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
                ExpiresIn = secondsExpire
            };
        }
    }
}
