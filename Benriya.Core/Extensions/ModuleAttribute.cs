using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Extensions
{
    public class ModuleAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                //context.Result = new UnauthorizedResult();
                //return;
                throw new UnauthorizedAccessException("Key: is required");
            }
            var client = new ClientInfomation(context.HttpContext);
            string apiKey = null;
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var value = await cache.GetAsync(CacheModel.ApiKey + client.GetClientID());
            if (value != null)
                apiKey = Encoding.UTF8.GetString(value);

            if (apiKey == null || !apiKey.Equals(potentialApiKey))
            {
                //context.Result = new UnauthorizedResult();
                //return;
                throw new UnauthorizedAccessException("Key is invalid, please reopen your appication");
            }
            await next();
        }
    }

    public class PermissionEditAttribute : Attribute
    {
        public string Permission { get; private set; }
        public PermissionEditAttribute()
        {
            Permission = this.GetAssemblyLastname();
        }
    }

    public class PermissionEdit
    {
        public static string Permission {get;set;}
        public PermissionEdit()
        {
            Permission = this.GetAssemblyLastname();
        }
    }

}
