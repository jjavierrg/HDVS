using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.PaisesController
{
    internal class PaisesControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.Paises.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 30; i++)
            {
                context.Paises.Add(new Pais { Id = i, Descripcion = $"Test {i}" });
            }

            await context.SaveChangesAsync();
        }
    }
}
