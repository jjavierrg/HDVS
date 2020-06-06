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
                OrganizacionId = 1,
                UsuarioId = 1,
                SexoId = 1,
                GeneroId= 1,
                MunicipioId = 1,
                ProvinciaId = 1,
                NacionalidadId = 1,
                OrigenId = 1,
                PadronId = 1
            };
        }
        public async Task Seed(HDVSContext context)
        {
            var lastId = await context.Fichas.AnyAsync() ? await context.Fichas.MaxAsync(x => x.Id) + 1 : 0;
            for (int i = lastId; i <= 20; i++)
            {
                context.Fichas.Add(GetNewFicha(i));
            }

            var lastAttachmentId = await context.Fichas.AnyAsync() ? await context.Fichas.MaxAsync(x => x.Id) + 1 : 0;
            for (int i = lastId; i <= 10; i++)
            {
                context.Adjuntos.Add(new Adjunto { Id = i, Alias = "Alias", FechaAlta = DateTime.Now, NombreOriginal = "NombreOriginal", Tamano = 123, TipoId = 1 });
            }

            await context.SaveChangesAsync();
        }
    }
}
