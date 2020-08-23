using EAPN.HDVS.Entities;
using EAPN.HDVS.Infraestructure.Repositories;
using EAPN.HDVS.Infrastructure.Core.Queries;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EAPN.HDVS.Infrastructure.Registry
{
    public static class Registry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadRepository<>), typeof(ReadRepository<>));

            services.AddScoped<IRepository<Usuario>, UsuarioRepository>();
            services.AddScoped<IRepository<Organizacion>, OrganizacionRepository>();
            services.AddScoped<IRepository<Ficha>, FichaRepository>();
            services.AddScoped<IRepository<Seguimiento>, SeguimientoRepository>();

            services.AddScoped<IQueryData, QueryData>();
            services.AddScoped(typeof(IFilterable<>), typeof(FilterPaginator<>));
            services.AddScoped(typeof(IPaginable<>), typeof(FilterPaginator<>));
            services.AddScoped(typeof(IFilterPaginable<>), typeof(FilterPaginator<>));

            return services;
        }
    }
}
