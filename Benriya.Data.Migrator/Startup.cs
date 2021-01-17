using System.Reflection;
using ExtCore.Data.Abstractions;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Benriya.Data.Context;
using Benriya.Share.Abstractions;
using Benriya.Core.Services;
using Benriya.Core.Extensions;

/*
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations add add_Example
dotnet ef migrations remove

dotnet tool install --global dotnet-ef --version 5.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design
*/

namespace Benriya.Data.Migrator {
    public class Startup {
        private readonly IWebHostEnvironment env;
        public IConfiguration Configuration { get; }
        private string extensionsPath;
        public Startup (IConfiguration configuration, IWebHostEnvironment env) {
            this.Configuration = configuration;
            this.env = env;
            extensionsPath = env.ContentRootPath + this.Configuration["Extensions:Path"];

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath + "/../Benriya.App")
                .AddJsonFile ("appsettings.json", optional : true, reloadOnChange : true);
            services.AddMapper();
            services.AddScoped<IRequestServices,RequestServices>();
            services.AddExtCore(extensionsPath, Configuration["Extensions:IncludingSubpaths"] == true.ToString());
            //services.AddDbContext<ApplicationDbContext>(options =>
            //  options.UseNpgsql(configurationBuilder.Build().GetConnectionString("Default")));
            var db_conector = configurationBuilder.Build().GetConnectionString("Default");
            //services.Configure<StorageContextOptions>(options =>
            //{
            //    options.ConnectionString = conect;
            //    options.MigrationsAssembly = Assembly.GetExecutingAssembly().FullName;
            //});
            services.AddScoped<IUserServices, UserServices>();
            services.AddSingleton<IRequestServices, RequestServices>();
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
            if (this.env.IsDevelopment()) { options.EnableSensitiveDataLogging(); }
                //options.UseLazyLoadingProxies(false);
                options.UseNpgsql(db_conector, (m) => { m.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); }); //(typeof(ApplicationDbContext).Assembly.FullName); }); // ("Benriya.Data.Migrator"); });
            });
            services.AddScoped(typeof(IStorageContext), typeof(ApplicationDbContext));           

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            DesignTimeStorageContextFactory.Initialize(services.BuildServiceProvider());
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            app.UseExtCore();
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory> ().CreateScope ()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate ();
                context.Database.EnsureCreated ();
            }

            app.UseRouting ();

            app.UseEndpoints (endpoints => {
                endpoints.MapGet ("/", async context => {
                    await context.Response.WriteAsync ("Migration app start...");
                });
            });
        }
    }
}
