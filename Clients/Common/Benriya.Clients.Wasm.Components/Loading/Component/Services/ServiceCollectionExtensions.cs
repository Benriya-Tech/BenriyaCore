using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Loading.Component.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoadingComponent(this IServiceCollection services)
        {
            return services.AddSingleton<ILoadingService, LoadingService>();
        }
    }
}
