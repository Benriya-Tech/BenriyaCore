using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components.Loading.Component.Services;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Share.ViewModels;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Shared.Authen
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public IAuthenServices _auth { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public ILoadingService _Loading { get; set; }
        [Inject]
        public ISessionStorageService _sessionStorage { get; set; }
        protected UserLoginModel model { get; set; } = new UserLoginModel() { email="xxx@yyy.com",password="123456"};
        protected UserInfoModel loggedIn { get; set; }
        protected string Message { get; set; }

        protected string AppVersion { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AppVersion = await _sessionStorage.GetItemAsync<string>(AppClient.VersionKey);
        }

        protected virtual async Task HandleValidSubmit()
        {
            bool reload = true;
            if (_navigationManager.Uri != null)
            {
                reload = !_navigationManager.Uri.ContainsAny(new string[] { "login", "logout", "register" },StringComparison.CurrentCultureIgnoreCase);
            }

            Message = null;
            _Loading.Show();
            var login = await _auth.Login(model);
            if (login)
            {
                //_navigationManager.NavigateTo("/");
                if (reload)
                    await JSRuntime.InvokeVoidAsync("window.location.reload");
                else
                    await JSRuntime.InvokeVoidAsync("window.location.replace", "/");

                Console.WriteLine(":) Logged in....");                
            }
            else
            {
                Message = _auth.Message;
                _Loading.Hide();
            }
            

        }
    }
}
