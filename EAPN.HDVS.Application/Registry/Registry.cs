using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Infrastructure.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace EAPN.HDVS.Application.Registry
{
    public static class Registry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICrudServiceBase<>), typeof(CrudServiceBase<>));
            services.AddScoped(typeof(IReadServiceBase<>), typeof(ReadServiceBase<>));
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}
