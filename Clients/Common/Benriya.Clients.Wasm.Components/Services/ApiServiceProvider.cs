using Benriya.Utils.Extensions;
using Benriya.Utils.Models;
using Blazored.SessionStorage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class ApiServiceProvider : IApiServiceProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IApiClientService<ApiClientInfo> _api;
        private readonly ISessionStorageService _sessionStorage;
        //private readonly string storageKey = "clientInfo";
        private int retry = 5;
        public ApiServiceProvider(HttpClient httpClient, ISessionStorageService sessionStorage, IApiClientService<ApiClientInfo> api)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
            _api = api;
        }

        public async Task IniClientInfo(bool reload = false)
        {
            try
            {
                string clientKey = "ClientKey";
                if (reload)
                    _httpClient.DefaultRequestHeaders.Remove("ClientKey");
                _httpClient.DefaultRequestHeaders.Remove("ApiKey");
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "xxxxxxxxxxxxxxxxxxx");
                var key = await _sessionStorage.GetItemAsync<string>(clientKey);
                if (!reload && !key.isNOEOW())
                {                    
                    var info = await _api.Get($"key/{key}");                    
                    if (info != null && info.Status == 200)
                    {
                        await _sessionStorage.RemoveItemAsync(clientKey);
                        await _sessionStorage.SetItemAsync(clientKey, key);
                        _httpClient.DefaultRequestHeaders.Add("clientKey", key);                        
                    }
                    else
                    {
                        _httpClient.DefaultRequestHeaders.Remove(clientKey);
                        _httpClient.DefaultRequestHeaders.Add(clientKey, key);
                    }
                }else if(reload || key.isNOEOW())
                {
                    await this.RequestKey(clientKey);
                }
                else if(!key.isNOEOW())
                {
                    _httpClient.DefaultRequestHeaders.Remove(clientKey);
                    _httpClient.DefaultRequestHeaders.Add(clientKey, key);
                }
                else
                {
                    await this.RequestKey(clientKey);
                }
            }
            catch (Exception)
            {
                if (retry > 0) {
                    await Task.Delay(300);
                    retry--;
                    await IniClientInfo(true);
                }
            }
        }

        private async Task RequestKey(string clientKey)
        {
            var info = await _api.Get("key");
            if (info != null && info.Status == 200 && info.Data != null && !info.Data.client_id.isNOEOW())
            {
                await _sessionStorage.RemoveItemAsync(clientKey);
                await _sessionStorage.SetItemAsync(clientKey, info.Data.key);
                _httpClient.DefaultRequestHeaders.Add("clientKey", info.Data.key);

            }
        }
        public async Task Reload()
        {
            await IniClientInfo(true);
        }

    }
}
