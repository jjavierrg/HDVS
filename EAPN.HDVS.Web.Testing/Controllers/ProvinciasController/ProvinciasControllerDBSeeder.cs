using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.ProvinciasController
{
    internal class ProvinciasControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.Provincias.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 30; i++)
            {
                context.Provincias.Add(new Provincia { Id = i, Nombre = $"Test {i}" });
            }

            await context.SaveChangesAsync();
        }
    }
}
