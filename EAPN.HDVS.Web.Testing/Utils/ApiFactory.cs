using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Testing.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EAPN.HDVS.Web.Testing.Utils
{
    public class ApiFactory<T> : WebApplicationFactory<T> where T : class
    {
        private readonly IList<ITestDBSeeder> _seeders;
        private readonly SqliteConnection _connection;
        private readonly Dictionary<string, string> _tokens;
        public MockupLogger<T> Logger { get; private set; }

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
                    {
                        services.Remove(descriptor);
                    }

                    // Add a database context using an in-memory database for testing.
                    services.AddDbContext<HDVSContext>(options =>
                    {
                        options.UseSqlite(_connection, x => { });
                    });

                    // Add logger
                    services.AddSingleton(typeof(ILogger<>), typeof(MockupLogger<>));

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;

                    Logger = (MockupLogger<T>)scopedServices.GetRequiredService<ILogger<T>>();

                    var context = scopedServices.GetRequiredService<HDVSContext>();
                    context.Database.EnsureCreated();

                    var passwordService = scopedServices.GetRequiredService<IPasswordService>();
                    var baseSeeder = new BaseTestDBSeeder(passwordService.HashPassword("pass"));

                    await baseSeeder.Seed(context);
                    foreach (var seeder in _seeders)
                    {
                        await seeder.Seed(context);
                    }

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

        public HttpClient GetAuthenticatedClient(string email)
        {
            var client = CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens[email.ToLower()]);
            return client;
        }

        public new void Dispose()
        {
            _connection.Close();
            base.Dispose();
        }
    }
}
