using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Reflection;

namespace Benriya.Core.Extensions
{
    public static class MapperCollectionExtensions
    {
        public static void AddExtCore(this IServiceCollection services)
        {
            services.AddMapper(null);
        }

        public static void AddMapper(this IServiceCollection services, Action<TypeAdapterConfig> options = null)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            //Mapper Config
            //------
            config.Scan(Assembly.GetExecutingAssembly());
            options?.Invoke(config);
            services.AddSingleton(config);
        }
    }
}
