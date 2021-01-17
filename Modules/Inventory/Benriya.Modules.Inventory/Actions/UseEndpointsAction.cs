using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Benriya.Modules.Inventory.Actions
{
  public class UseEndpointsAction : IUseEndpointsAction
  {
    public int Priority => 150;

    public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
    {
      endpointRouteBuilder.MapControllerRoute(name: "Default", pattern: "{controller}/{action}", defaults: new { controller = "Default", action = "Index" });
    }
  }
}