using Benriya.Utils;
//using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public interface IApiClientService<T>
    {
        Task<ApiResultModel<T>> Delete(string uri);
        Task<ApiResultModel<T>> Get(string uri);
        Task<ApiPagingtModel<List<T>>> GetList(string uri);
        Task<ApiResultModel<T>> Patch(string uri, HttpContent content);
        Task<ApiResultModel<T>> Post(string uri, T data);
        Task<ApiResultModel<T>> Update(string uri, T data);
        Task<ApiDropdownModel> GetDropdown(string uri);
        Task<ApiResultModel<T1>> GetCustom<T1>(string uri);
        Task<ApiResultModel<T1>> PostCustom<T1>(string uri, T data);
        Task<ApiResultModel<T1>> UpdateCustom<T1>(string uri, T data);
        Task<ApiResultModel<T1>> UploadFile<T1>(string uri, MultipartFormDataContent content);
        Task<string> GetFile(string uri);
        bool checkClient { get; set; }

    }
}