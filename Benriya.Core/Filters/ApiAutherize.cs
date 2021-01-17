using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Filters
{
    public class ApiAuthurize : Attribute, IAsyncActionFilter
    {
        private const string ClientKeyHeaderName = "clientKey";
        private const string ClientApiHeaderName = "apiKey";
        private bool isCheckCleint = true;

        public ApiAuthurize(bool checkClient = true)
        {
            isCheckCleint = checkClient;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var result = new ApiResultModel<bool>();
            if (isCheckCleint)
            {
                if (!context.HttpContext.Request.Headers.TryGetValue(ClientKeyHeaderName, out var potentialClientKey))
                {
                    result = new ApiResultModel<bool>();
                    result.Unauthorized("Client Key: is required");             
                    context.Result = new JsonResult(result);
                    context.HttpContext.Response.StatusCode = result.Status;
                    return;
                }
                var client = new ClientInfomation(context.HttpContext);
                string clientKey = null;
                var value = await cache.GetAsync(CacheModel.ApiKey + client.GetClientID());
                if (value != null)
                    clientKey = Encoding.UTF8.GetString(value);

                if (clientKey == null || !clientKey.Equals(potentialClientKey))
                {
                    result = new ApiResultModel<bool>();
                    result.Unauthorized("Client Key is invalid, please reopen your appication");
                    context.Result = new JsonResult(result);
                    context.HttpContext.Response.StatusCode = result.Status;
                    return;
                }
            }else if (!context.HttpContext.Request.Headers.TryGetValue(ClientApiHeaderName, out var potentialClientApi))
            {
                result = new ApiResultModel<bool>();
                result.Unauthorized("API Key: is required");
                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = result.Status;
                return;
            }
            await next();
        }
    }
}
