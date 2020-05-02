using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EAPN.HDVS.Infrastructure.Registry
{
    public static class Registry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadRepository<>), typeof(ReadRepository<>));
            return services;
        }
    }
}
