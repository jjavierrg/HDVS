using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Application.Repositories;
using EAPN.HDVS.Application.Services.Dimension;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EAPN.HDVS.Application.Registry
{
    public static class Registry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Usuario>, UsuarioRepository>();
            services.AddScoped<IRepository<Organizacion>, OrganizacionRepository>();
            services.AddScoped<IRepository<Ficha>, FichaRepository>();

            services.AddScoped(typeof(ICrudServiceBase<>), typeof(CrudServiceBase<>));
            services.AddScoped(typeof(IReadServiceBase<>), typeof(ReadServiceBase<>));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IDimenssionService, DimenssionService>();

            return services;
        }
    }
}
