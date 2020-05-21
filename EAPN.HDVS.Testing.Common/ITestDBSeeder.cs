
using EAPN.HDVS.Infrastructure.Context;
using System.Threading.Tasks;

namespace EAPN.HDVS.Testing.Common
{
    public interface ITestDBSeeder
    {
        void Seed(HDVSContext context);
    }
}
