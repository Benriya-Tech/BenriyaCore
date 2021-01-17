using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Benriya.Modules.eCommerce.Actions
{
  public class UseEndpointsAction : IUseEndpointsAction
  {
    public int Priority => 200;

    public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
    {
      endpointRouteBuilder.MapControllerRoute(name: "Default", pattern: "report/{controller}/{action}", defaults: new { controller = "Default", action = "Index" });
    }
  }
}