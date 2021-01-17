
//using System;
//using ExtCore.Data.Abstractions;
//using ExtCore.Infrastructure.Actions;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Benriya.Context;

//namespace Benriya.Modules.Core.Actions
//{
//    public class ConfigureServicesAction : IConfigureServicesAction
//    {
//        public int Priority => 1000;

//        public void Execute(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
//        {
//            // This is a bad (but quick) way to provide configurations to the extensions. A good one is to use the options pattern.
//            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
//            //  .SetBasePath(serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath)
//            //  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

//            //serviceCollection.AddDbContext<ApplicationDbContext>(options =>
//            //options.UseNpgsql(configurationBuilder.Build().GetConnectionString("Default")));

//            //serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
//            //  .AddEntityFrameworkStores<ApplicationDbContext>()
//            //  .AddDefaultTokenProviders();

//            //serviceCollection.AddScoped(typeof(IStorageContext), typeof(ApplicationDbContext));
//        }
//    }
//}
