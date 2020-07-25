using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Controllers.SituacionesAdministrativasController
{
    internal class SituacionesAdministrativasDBSeeder : ITestDBSeeder
    {
        public async Task Seed(HDVSContext context)
        {
            var lastId = context.SituacionesAdministrativas.Max(x => x.Id) + 1;
            for (int i = lastId; i <= 30; i++)
            {
                context.SituacionesAdministrativas.Add(new SituacionAdministrativa { Id = i, Descripcion = $"SituacionAdministrativa {i}" });
            }

            await context.SaveChangesAsync();
        }
    }
}
