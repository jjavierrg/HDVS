
using EAPN.HDVS.Infrastructure.Context;
using System.Threading.Tasks;

namespace EAPN.HDVS.Testing.Common
{
    public interface ITestDBSeeder
    {
        Task Seed(HDVSContext context);
    }
}
