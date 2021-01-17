using Benriya.Utils;
using Benriya.Utils.Extensions;
using Blazored.LocalStorage;
//using Microsoft.Extensions.FileProviders;
using Tewr.Blazor.FileReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using Benriya.Share.ViewModels;
using System.Net.Http.Headers;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class ApiClientService<T> : IApiClientService<T>
    {
        
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        //private readonly ISessionStorageService _sessionStorage;
        //private readonly ApiServiceProvider _apiProvider;
        //private readonly AuthenticationStateProvider _authenStateProvider;
        //private readonly string storageKey = "authToken";
        private bool is_setClient { get; set; }
        public bool checkClient { get; set; }
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        public ApiClientService(HttpClient httpClient,ILocalStorageService localStorage)//, ISessionStorageService sessionStorage)
        {
            //_authenStateProvider = authenStateProvider;
            //_sessionStorage = sessionStorage;
            //_apiProvider = apiProvider; ApiServiceProvider apiProvider,
            //SetClient();            
            _httpClient = httpClient;
            _localStorage = localStorage;

            //if (checkClient && !is_setClient)
            //{
            //    _sessionStorage = sessionStorage;
            //    SetClient();
            //}
        }

        //private async void SetClient()
        //{
        //    string clientKey = "clientKey";
        //    var key = await _sessionStorage.GetItemAsync<string>(clientKey);
        //    if (!clientKey.isNOEOW())
        //    {
        //        _httpClient.DefaultRequestHeaders.Remove(clientKey);
        //        _httpClient.DefaultRequestHeaders.Add(clientKey, key);
        //    }
        //}

        public async Task<ApiResultModel<T>> Get(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T>>().Result;
            }
            catch (Exception)
            {
                return await this.checkData(response);
            }
        }

        public async Task<ApiResultModel<T1>> GetCustom<T1>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T1>>().Result;
            }
            catch (Exception)
            {
                if (response != null && response.Content != null)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    if (!contents.isNOEOW() && contents.IsValidJson())
                        return JsonConvert.DeserializeObject<ApiResultModel<T1>>(contents, settings);
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public async Task<ApiPagingtModel<List<T>>> GetList(string uri)
        {
            var response =   await _httpClient.GetAsync(uri);
            try {
                return response.Content.ReadFromJsonAsync<ApiPagingtModel<List<T>>>().Result;
            }
            catch (Exception)
            {
                return await this.checkDataPaging(response);             
            }
        }

        public async Task<ApiResultModel<T>> Post(string uri, T data)
        {
            var response =  await _httpClient.PostAsJsonAsync(uri,data);
            try {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T>>().Result;
            }
            catch (Exception)
            {
                return await this.checkData(response);
                
            }   
        }

        public async Task<ApiResultModel<T1>> UploadFile<T1>(string uri,MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync(uri, content);
            try
            {                
                return response.Content.ReadFromJsonAsync<ApiResultModel<T1>>().Result;
            }
            catch (Exception)
            {
                if (response != null && response.Content != null)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    if (!contents.isNOEOW() && contents.IsValidJson())
                        return JsonConvert.DeserializeObject<ApiResultModel<T1>>(contents, settings);
                    else
                        return null;
                }
                else
                    return null;
            }
        }


        public async Task<ApiResultModel<T1>> PostCustom<T1>(string uri, T data)
        {
            var response = await _httpClient.PostAsJsonAsync(uri, data);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T1>>().Result;
            }
            catch (Exception)
            {
                if (response != null && response.Content != null)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    if (!contents.isNOEOW() && contents.IsValidJson())
                        return JsonConvert.DeserializeObject<ApiResultModel<T1>>(contents, settings);
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public async Task<ApiResultModel<T>> Update(string uri, T data)
        {
            var response = await _httpClient.PutAsJsonAsync(uri, data);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T>>().Result;
            }
            catch (Exception)
            {
                return await this.checkData(response);            
            }
        }
        public async Task<ApiResultModel<T1>> UpdateCustom<T1>(string uri, T data)
        {
            var response = await _httpClient.PutAsJsonAsync(uri, data);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T1>>().Result;
            }
            catch (Exception)
            {
                if (response != null && response.Content != null)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    if (!contents.isNOEOW() && contents.IsValidJson())
                        return JsonConvert.DeserializeObject<ApiResultModel<T1>>(contents, settings);
                    else
                        return null;
                }
                else
                    return null;
            }
        }


        public async Task<ApiResultModel<T>> Delete(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            //response.EnsureSuccessStatusCode();
            try
            {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T>>().Result;
            }catch(Exception)
            {
                return await this.checkData(response);            
            }
        }

        public async Task<ApiResultModel<T>> Patch(string uri, HttpContent content)
        {
            var response = await _httpClient.PatchAsync(uri, content);
            try {
                return response.Content.ReadFromJsonAsync<ApiResultModel<T>>().Result;
            }
            catch (Exception)
            {
                return await this.checkData(response);     
            }
        }



        public async Task<ApiDropdownModel> GetDropdown(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            try
            {
                return response.Content.ReadFromJsonAsync<ApiDropdownModel>().Result;
            }
            catch (Exception)
            {
                if (response != null && response.Content != null)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    if (!contents.isNOEOW() && contents.IsValidJson())
                        return JsonConvert.DeserializeObject<ApiDropdownModel>(contents, settings);
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public async Task<string> GetFile(string uri)
        {
            var response = await _httpClient.GetByteArrayAsync(uri);
            try
            {
                return Convert.ToBase64String(response);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        private async Task<ApiResultModel<T>> checkData(HttpResponseMessage response)
        {
            if (response != null && response.Content != null)
            {
                string contents = await response.Content.ReadAsStringAsync();
                if (!contents.isNOEOW() && contents.IsValidJson())
                {

                    return JsonConvert.DeserializeObject<ApiResultModel<T>>(contents, settings);
                }
                else
                {
                    Console.WriteLine(contents);
                    return null;
                }
            }
            else
                return null;
        }
        
        private async Task<ApiPagingtModel<List<T>>> checkDataPaging(HttpResponseMessage response)
        {
            if (response != null && response.Content != null)
            {
                string contents = await response.Content.ReadAsStringAsync();
                if (!contents.isNOEOW() && contents.IsValidJson())
                    return JsonConvert.DeserializeObject<ApiPagingtModel<List<T>>>(contents, settings);
                else
                    return null;
            }
            else
                return null;
        }

        private async Task<string> GetClientID()
        {
            return await _localStorage.GetItemAsStringAsync("ClientID");
        }
    }
}
