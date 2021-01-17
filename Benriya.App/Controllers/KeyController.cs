using Benriya.Core.Classes;
using Benriya.Core.Filters;
using Benriya.Core.Services;
using Benriya.Share.Abstractions;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class KeyController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private IRequestServices _request { get; set; }
        private HttpContext _httpContext;
        private ClientInfomation _client;
        private string client_id;
        public KeyController(IDistributedCache cache,IRequestServices request)
        {
            _cache = cache;
            _request = request;
        }
        [HttpGet]
        [ApiAuthurize(false)]
        public async Task<IActionResult> Index()
        {
            _httpContext = HttpContext;
            _client = new ClientInfomation(_httpContext);
            client_id = _client.GetClientID();
            var result = new ApiResultModel<ApiClientInfo>();
            var keystore = await _cache.GetAsync(CacheModel.ApiKey+client_id);
            if (keystore != null)
            {
                return GetKey(keystore);
            }
            else
            {
                await new ClientKey(_cache, _httpContext, _request).SetKey();
                keystore = await _cache.GetAsync(CacheModel.ApiKey + client_id);
                return GetKey(keystore);
            }
        }

        private IActionResult GetKey(byte[] keystore)
        {
            var result = new ApiResultModel<ApiClientInfo>();
            if (keystore != null)
            {
                result.Data = new ApiClientInfo() { ip = _client.GetClientIP(), ua = _client.GetUserAgent(), client_id = client_id, key = Encoding.UTF8.GetString(keystore) };
                return Ok(result);
            }
            else
            {
                result.BadRequest("Cannot generate  API key");
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("{key}")]
        public  async Task<IActionResult> Check(string key)
        {
            var result = new ApiResultModel<bool>();
            _client = new ClientInfomation(HttpContext);
            var keystore = await _cache.GetAsync(CacheModel.ApiKey+_client.GetClientID());
            if (keystore != null)
            {
                if (Encoding.UTF8.GetString(keystore).Equals(key)) {
                    result.Data = true;
                    return Ok(result);
                }
                else
                {
                    result.Notfound();
                    result.Data = false;
                    return NotFound(result);
                }
            }
            else
            {
                result.Notfound("API key not found");
                return NotFound(result);
            }
        }
    }
}
