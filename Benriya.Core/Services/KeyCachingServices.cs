using Benriya.Share.Abstractions;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Services
{
    public class KeyCachingServices : IKeyCachingServices
    {
        private readonly IDistributedCache _cache;
        private readonly AppSettings _appSetting;
        public IRequestServices _RequestServices { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public KeyCachingServices(IDistributedCache cache, IOptions<AppSettings> appSetting, IHttpContextAccessor httpContextAccessor, IRequestServices rq)
        {
            _cache = cache;
            _appSetting = appSetting?.Value;
            _httpContextAccessor = httpContextAccessor;
            _RequestServices = rq;
        }


        public async Task SetKey(HttpContext _httpContext = null)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (_httpContext != null)
                httpContext = _httpContext;
            var client = new ClientInfomation(httpContext);
            string val = null;
            string refkey = CacheModel.ApiKey + client.GetClientID();
            var value = await _cache.GetAsync(refkey);
            if (value != null)
                val = Encoding.UTF8.GetString(value);
            else
            {
                val = RNG.UniqueString();
                await _cache.SetAsync(refkey, Encoding.UTF8.GetBytes(val));
            }
            if (_RequestServices != null)
                _RequestServices.SetClientInfo(new ClientInfo()
                {
                    id = refkey,
                    key = val,
                    ipAddress = client.GetClientIP(),
                    ConnectionID = client.GetConnectionID(),
                    UserAgent = client.GetUserAgent()
                });
            httpContext.Response.Headers.Append("clientKey", val);
        }
    }


}
