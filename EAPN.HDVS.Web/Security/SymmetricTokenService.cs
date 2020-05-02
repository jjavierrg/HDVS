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
        public SymmetricTokenService(ITokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        public int GetRefreshTokenLifeMinutes()
        {
            return _tokenConfiguration?.RefreshTokenLifeMinutes ?? (24 * 60);
        }

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
            });

            var secondsExpire = Math.Max(_tokenConfiguration.TokenLifeMinutes, 10) * 60;

            if (permisos.Any())
                claims.AddRange(permisos.Select(x => new Claim(ClaimTypes.Role, x.Descripcion)));
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
                RefreshToken = GenerateToken()
            };
        }

        private string GenerateToken()
        {
            int size = 64;
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
