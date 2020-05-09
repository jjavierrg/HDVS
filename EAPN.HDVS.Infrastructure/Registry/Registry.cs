using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Infrastructure.Core.Queries;
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

            services.AddScoped<IQueryData, QueryData>();
            services.AddScoped(typeof(IFilterable<>), typeof(FilterPaginator<>));
            services.AddScoped(typeof(IPaginable<>), typeof(FilterPaginator<>));
            services.AddScoped(typeof(IFilterPaginable<>), typeof(FilterPaginator<>));

            return services;
        }
    }
}
