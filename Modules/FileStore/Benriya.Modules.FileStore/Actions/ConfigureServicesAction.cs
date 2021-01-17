using System;
using Benriya.Core.Classes;
using ExtCore.FileStorage.Abstractions;
using ExtCore.FileStorage.FileSystem;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Benriya.Modules.FileStore.Actions
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 1002;

        public void Execute(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {            
            serviceCollection.AddTransient<FileSystemManager>();
            serviceCollection.AddScoped(typeof(IFileStorage), typeof(FileStorage));
        }
    }
}
