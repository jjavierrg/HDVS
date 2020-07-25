using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.MunicipiosController
{
    internal class MunicipiosControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.Municipios.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 250; i++)
            {
                context.Municipios.Add(new Municipio { Id = i, Nombre = $"Test {i}", ProvinciaId = (i % 10) + 1 });
            }

            await context.SaveChangesAsync();
        }
    }
}
