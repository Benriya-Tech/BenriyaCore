using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using ExtCore.Mvc.Infrastructure.Actions;

namespace Benriya.Modules.Core.Actions
{
    public class UseEndpointsAction : IUseEndpointsAction
    {
        public int Priority => 1001;

        public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
        {
            //endpointRouteBuilder.MapControllerRoute(name: "Module CMS", pattern: "", defaults: new { controller = "ModuleCMS", action = "Index" });
            endpointRouteBuilder.MapControllerRoute(name: "Module Core API", pattern: "{controller}/{action}", defaults: new { controller = "Default", action = "Index" });
        }
    }
}
