using Benriya.Share.ViewModels;
using Benriya.Utils.Extensions;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class AuthenServices : IAuthenServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService  _sessionStorage;
        private readonly IApiClientService<UserLoginModel> _api;
        private readonly AuthenticationStateProvider _authenStateProvider;
        
        private readonly string storageKey = "authToken";
        public string Message { get; set; }
        public bool isLoggedIn { get; set; }
        
        public AuthenServices(ILocalStorageService localStorage,ISessionStorageService sessionStorage,AuthenticationStateProvider authenStateProvider, IApiClientService<UserLoginModel> api,HttpClient httpClient)
        {
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
            _api = api;
            _httpClient = httpClient;
            _authenStateProvider = authenStateProvider;
            checktUser();
        }

        public async Task<bool> Login(UserLoginModel idata)
        {
            Message = null;
            isLoggedIn = false;
            var response = await _api.PostCustom<UserInfoModel>($"core/Authorize", idata);
            if (response != null)
            {
                if (response.Status == 200)
                {
                    if (response.Data != null && response.Data.id != Guid.Empty && response.Data.Token != null && !response.Data.Token.token.isNOEOW())
                    {
                        await _sessionStorage.SetItemAsync(storageKey, response.Data);
                        ((ApiAuthenticationStateProvider)_authenStateProvider).MarkUserAsAuthenticated(response.Data.Token.token);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Data.Token.token);
                        isLoggedIn = true;
                        KeepSession();
                    }
                    else
                    {
                        Message = response.Message.isNOEOW() ? "Cannot retrive data" : response.Message;
                    }
                }
                else
                {
                    Message = response.Message;
                }
            }
            else
            {
                Message = "Cannot provide login data";
            }            
            return isLoggedIn;
        }

        public async Task<bool> Logout()
        {
            Message = null;

            await _sessionStorage.RemoveItemAsync(storageKey);
            ((ApiAuthenticationStateProvider)_authenStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return true;
        }

        public async Task<UserInfoModel> GetUser()
        {
            Message = null;
            var user =  await _sessionStorage.GetItemAsync<UserInfoModel>(storageKey);    
            isLoggedIn =  user != null && user.id != Guid.Empty && user.Token != null && !user.Token.token.isNOEOW() && user.Token.expiry > DateTime.Now;

            //await ((ApiAuthenticationStateProvider)_authenStateProvider).GetAuthenticationStateAsync();
            return user;
        }
        private async void checktUser()
        {
            Message = null;
            await GetUser();      
            KeepSession();
        }

        public async Task<string> GetToken()
        {
            Message = null;
            var user = await _sessionStorage.GetItemAsync<UserInfoModel>(storageKey);
            return user.Token.token;
        }

        public async Task<bool> ReFreshToken()
        {
            Message = null;
            await _sessionStorage.RemoveItemAsync(storageKey);
            return true;
        }

        public async Task<bool> checkLoggedIn()
        {
            try
            {
                Message = null;
                await GetUser();
            }
            catch (Exception)
            {
                isLoggedIn = false;
            }
            return isLoggedIn;
        }

        private void OnAsyncMethodFailed(Task task)
        {
            Exception ex = task.Exception;
            Console.WriteLine(ex.Message);
        }
        
        private int counter = 0;
        private async void KeepSession()
        {
            while (isLoggedIn && counter < 10)
            {
                Console.WriteLine("counter: "+counter+", isLoggedIn:"+isLoggedIn);
                if (counter == 8)
                {
                    await Logout();
                }
                await Task.Delay(60000);  // sleep for a while.
                counter++;
            }
        }
    }
}
