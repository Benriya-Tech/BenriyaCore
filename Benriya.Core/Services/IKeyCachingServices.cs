using Benriya.Share.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Benriya.Core.Services
{
    public interface IKeyCachingServices
    {
        IRequestServices _RequestServices { get; set; }

        Task SetKey(HttpContext _httpContext = null);
    }
}