using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Benriya.Modules.Default.Actions
{
  public class UseEndpointsAction : IUseEndpointsAction
  {
    public int Priority => 99;

    public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
    {
      endpointRouteBuilder.MapControllerRoute(name: "Default", pattern: "modules/{controller}/{action}", defaults: new { controller = "Default", action = "Index" });
    }
  }
}