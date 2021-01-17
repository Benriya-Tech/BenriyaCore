using Benriya.Share.Abstractions;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Classes
{
    public class ClientKey
    {
        private IDistributedCache _cache;
        private HttpContext _httpContext;
        private IRequestServices _RequestServices { get; set; }
        public ClientKey(IDistributedCache cache,HttpContext httpContext,IRequestServices requestServices)
        {
            _cache = cache;
            _httpContext = httpContext;
            _RequestServices = requestServices;
        }

        public async Task SetKey()
        {
            var client = new ClientInfomation(_httpContext);
            string val = null;
            string refkey = CacheModel.ApiKey + client.GetClientID();
            var value = await _cache.GetAsync(refkey);
            if (value != null)
                val = Encoding.UTF8.GetString(value);
            else
            {
                val = CryptographyCore.SHA256_hash(Guid.NewGuid().ToString());
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
            _httpContext.Response.Headers.Append("clientKey", val);
        }
    }
}
