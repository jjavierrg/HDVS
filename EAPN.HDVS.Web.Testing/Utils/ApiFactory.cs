using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using EAPN.HDVS.Web.Dto.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Testing.Utils
{
    public class ApiFactory<T> : WebApplicationFactory<T> where T : class
    {
        private readonly IList<ITestDBSeeder> _seeders;
        private readonly SqliteConnection _connection;
        private readonly Dictionary<string, string> _tokens;

        public ApiFactory() : base()
        {
            // create sqlite connection
            _connection = new SqliteConnection("datasource=:memory:");
            _connection.Open();

            _seeders = new List<ITestDBSeeder>();
            _tokens = new Dictionary<string, string>();
        }

        public void AddAdditionalSeeder(ITestDBSeeder seeder)
        {
            _seeders.Add(seeder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(async services =>
                {
                    // Remove the app's ApplicationDbContext registration.
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HDVSContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add a database context using an in-memory database for testing.
                    services.AddDbContext<HDVSContext>(options =>
                    {
                        options.UseSqlite(_connection, x => { });
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;

                    var context = scopedServices.GetRequiredService<HDVSContext>();
                    context.Database.EnsureCreated();

                    var passwordService = scopedServices.GetRequiredService<IPasswordService>();
                    var baseSeeder = new BaseTestDBSeeder(passwordService.HashPassword("pass"));

                    baseSeeder.Seed(context);
                    foreach (var seeder in _seeders)
                        seeder.Seed(context);

                    await context.SaveChangesAsync();

                    // Generate tokens
                    _tokens.Clear();
                    var tokenService = scopedServices.GetRequiredService<ITokenService>();
                    foreach (var usuario in context.Usuarios.Include(x => x.PermisosAdicionales).ThenInclude(x => x.Permiso).Include(x => x.Perfiles).ThenInclude(x => x.Perfil).ThenInclude(x => x.Permisos).ThenInclude(x => x.Permiso))
                    {
                        var token = tokenService.GenerateTokenForUser(usuario);
                        _tokens.Add(usuario.Email.ToLower(), token.AccessToken);
                    }

                })
                .UseEnvironment("Test");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string email)
        {
            var client = CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens[email.ToLower()]);
            return client;
        }

        public void Dispose()
        {
            _connection.Close();
            base.Dispose();
        }
    }
}
