using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Benriya.App
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            }).UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();//.UseSerilog((hostingContext, loggerConfig) =>
            //loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));
        });
        
        #region Orifinal
        /*
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        */
        #endregion endif original
    }
}