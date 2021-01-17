using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using ExtCore.Mvc.Infrastructure.Actions;

namespace Benriya.Modules.FileStore.Actions
{
    public class UseEndpointsAction : IUseEndpointsAction
    {
        public int Priority => 1002;

        public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
        {
            endpointRouteBuilder.MapControllerRoute(name: "FileStore", pattern: "{controller}/{action}", defaults: new { controller = "Default", action = "Index" });
        }
    }
}
