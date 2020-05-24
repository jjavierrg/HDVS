using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.UsuariosController
{
    internal class UsuariosControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastUserId = context.Usuarios.Max(x => x.Id) + 1;
            for (int i = lastUserId; i <= 30; i++)
            {
                // Additional users
                context.Usuarios.Add(new Usuario
                {
                    Activo = true,
                    Apellidos = $"Apellidos",
                    OrganizacionId = 1,
                    Email = $"test{i}@test.com",
                    Hash = "",
                    Id = i,
                    Nombre = $"Nombre",
                    Perfiles = new List<UsuarioPerfil>(new[] { new UsuarioPerfil { PerfilId = 1, UsuarioId = i } }),
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
