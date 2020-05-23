using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.PerfilesController
{
    internal class PerfilesControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            // profile with superadmin permission
            context.Perfiles.Add(new Perfil { Id = 4, Descripcion = "Super administrador" });
            context.PerfilesPermisos.Add(new PerfilPermiso { PerfilId = 4, PermisoId = 6 });

            await context.SaveChangesAsync();
        }
    }
}
