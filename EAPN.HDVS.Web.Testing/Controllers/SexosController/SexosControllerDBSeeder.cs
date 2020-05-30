using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.SexosController
{
    internal class SexosControllerDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.Sexos.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 30; i++)
            {
                context.Sexos.Add(new Sexo { Id = i, Descripcion = $"Test {i}" });
            }

            await context.SaveChangesAsync();
        }
    }
}
