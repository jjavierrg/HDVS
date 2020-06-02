using AutoMapper;
using EAPN.HDVS.Application.Models;
using EAPN.HDVS.Application.Registry;
using EAPN.HDVS.Application.Security;
using EAPN.HDVS.Infrastructure.Context;
using EAPN.HDVS.Infrastructure.Registry;
using EAPN.HDVS.Web.Security;
using EAPN.HDVS.Web.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace EAPN.HDVS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            configureLogger();
            services.AddControllersWithViews();
            ConfigureAuthentication(services);
            ConfigureDataAccess(services);
            services.AddAutoMapper(typeof(Startup));
            services.AddInfrastructure();
            services.AddApplicationServices();
            ConfigureCors(services);
#if (DEBUG)
            ConfigureSwagger(services);
            LogManager.GlobalThreshold = LogLevel.Off;
#endif

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HDVS API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    scope.ServiceProvider.GetService<DbContext>().Database.Migrate();
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private void configureLogger()
        {
            GlobalDiagnosticsContext.Set("myDataBase", Configuration.GetConnectionString("HDVSDatabase"));
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile(path: "AppSettings.json").Build();
            NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = config;
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HDVS API",
                    Description = "HDVS Web API",
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "`Token only!!!` - without `Bearer_` prefix",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            // Login Process Configuration
            var minutes = Configuration.GetValue<int>("Login:MinutesBlock");
            var config = new LoginConfiguration
            {
                MaxLoginAttemp = Configuration.GetValue<int>("Login:MaxLoginAttemp"),
                BlockTime = TimeSpan.FromMinutes(Math.Max(minutes, 1))
            };

            // Token Configuration
            var authConfig = new TokenConfiguration();
            Configuration.Bind("Authentication", authConfig);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrWhiteSpace(authConfig.Issuer),
                    ValidateAudience = !string.IsNullOrWhiteSpace(authConfig.Audience),
                    ValidateLifetime = authConfig.TokenLifeMinutes > 0,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SymmetricSecret))
                };
            });

            // Register Configurations
            services.AddSingleton<ILoginConfiguration>(config);
            services.AddSingleton<ITokenConfiguration>(authConfig);
            services.AddTransient<ITokenService, SymmetricTokenService>();
            services.AddTransient<IPasswordService, BCryptPasswordService>();
        }

        private void ConfigureDataAccess(IServiceCollection services)
        {
            services.AddTransient<DbContext, HDVSContext>();
            services.AddDbContext<HDVSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HDVSDatabase"), x => x.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)));
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var AllowedHosts = Configuration.GetSection("AllowedCorsOrigins").Value.Split(",");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(AllowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
    }
}
