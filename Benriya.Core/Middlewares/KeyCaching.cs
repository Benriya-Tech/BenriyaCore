using Benriya.Core.Abstractions;
using Benriya.Core.Services;
using Benriya.Share.Abstractions;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Middlewares
{
    public class KeyCaching
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;
        private readonly AppSettings _appSetting;
        public IRequestServices RequestServices { get; set; }

        public KeyCaching(RequestDelegate next, IDistributedCache cache, IOptions<AppSettings> appSetting)
        {
            _next = next;
            _cache = cache;
            _appSetting = appSetting?.Value;            
        }

        public async Task Invoke(HttpContext httpContext,IRequestServices rq)
        {
            RequestServices = rq;

            var client = new ClientInfomation(httpContext);
            string val = null;
            string refkey = CacheModel.ApiKey + client.GetClientID();
            var value = await _cache.GetAsync(refkey);
            if (value != null)            
                val = Encoding.UTF8.GetString(value);            
            else
            {
                // load form db                
                val =  CryptographyCore.SHA256_hash(Guid.NewGuid().ToString());
                await _cache.SetAsync(refkey,Encoding.UTF8.GetBytes(val));
                // store to db
            }
            if(RequestServices != null)
                RequestServices.SetClientInfo(new ClientInfo() {
                    id = refkey,key = val,ipAddress = client.GetClientIP(),
                    ConnectionID = client.GetConnectionID(),UserAgent = client.GetUserAgent()
                });
            httpContext.Response.Headers.Append("ClientKey", val);
            if(_next != null && httpContext != null)
                await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class KeyCachingExtensions
    {
        public static IApplicationBuilder UseKeyCaching(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<KeyCaching>();
        }
    }
}
