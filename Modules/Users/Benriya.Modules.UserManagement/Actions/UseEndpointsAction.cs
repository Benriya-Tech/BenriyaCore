using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Benriya.Modules.UserManagement.Actions
{
    public class UseEndpointsAction : IUseEndpointsAction
    {
        public int Priority => 2;

        public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
        {
            //endpointRouteBuilder.MapControllerRoute(name: "Module Users", pattern: "users", defaults: new { controller = "ModuleUsers", action = "Index" });
            endpointRouteBuilder.MapControllerRoute(name: "Module Users", pattern: "{controller}/{action}");
        }
    }
}
