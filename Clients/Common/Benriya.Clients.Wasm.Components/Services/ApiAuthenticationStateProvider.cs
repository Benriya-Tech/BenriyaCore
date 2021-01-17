using Benriya.Share.ViewModels;
using Benriya.Utils.Extensions;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService _sessionStorage;
        private readonly string storageKey = "authToken";
        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _sessionStorage.GetItemAsync<UserInfoModel>(storageKey);
            if (savedToken == null || savedToken.Token == null || savedToken.Token.token.isNOEOW())
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            //try
            //{
            //    string clientKey = "clientKey";
            //    var key = await _sessionStorage.GetItemAsync<string>(clientKey);
            //    if (!key.isNOEOW())
            //    {
            //        _httpClient.DefaultRequestHeaders.Remove(clientKey);
            //        _httpClient.DefaultRequestHeaders.Add(clientKey, key);
            //    }
            //}
            //catch (Exception) { }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken.Token.token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken.Token.token), "jwt")));
        }

        //public void MarkUserAsAuthenticated(string email)
        //{
        //    var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwt"));
        //    var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        //    NotifyAuthenticationStateChanged(authState);
        //}

        public void MarkUserAsAuthenticated(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            //Console.WriteLine(authenticatedUser);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            Console.WriteLine("------------ Count roles: "+ClaimTypes.Role);
            Console.WriteLine(keyValuePairs.Count());
            keyValuePairs.TryGetValue("role", out object roles);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(jsonBytes));
            
            if (roles != null)
            {
                Console.WriteLine(roles.ToString());
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
