using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.FichasController
{
    internal class FichasControllerDBSeeder : ITestDBSeeder
    {
        private Ficha GetNewFicha(int id)
        {
            return new Ficha
            {
                Id = id,
                Nombre = "Nombre",
                Codigo = "Codigo",
                FotocopiaDNI = false,
                PoliticaFirmada = false,
                Apellido1 = "Apellido1",
                Apellido2 = "Apellido2",
                OrganizacionId = 0,
                UsuarioId = 0
            };
        }
        public async Task Seed(HDVSContext context)
        {
            var lastId = await context.Adjuntos.AnyAsync() ? await context.Adjuntos.MaxAsync(x => x.Id) + 1 : 0;
            for (int i = lastId; i <= 10; i++)
            {
                context.Adjuntos.Add(new Adjunto { Id = i, Alias = "Alias", FechaAlta = DateTime.Now, NombreOriginal = "NombreOriginal", Tamano = 123, TipoId = 1 });
            }

            await context.SaveChangesAsync();
        }
    }
}
