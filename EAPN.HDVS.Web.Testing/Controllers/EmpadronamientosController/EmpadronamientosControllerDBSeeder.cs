using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.EmpadronamientosController
{
    internal class EmpadronamientosControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.Empadronamientos.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 30; i++)
            {
                context.Empadronamientos.Add(new Empadronamiento { Id = i, Descripcion = $"Test {i}" });
            }

            await context.SaveChangesAsync();
        }
    }
}
