using Benriya.Clients.Wasm.Components.Services;
using Benriya.Share.Models.Menus;
using Benriya.Utils.Extensions;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Classes.Layout
{
    public class NavMenu : ComponentBase
    {
        [Inject]
        public IApiClientService<List<SystemMenu>> _api { get; set; }
        [Inject]
        public IToastService _toastService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public List<SystemMenu> menuList { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnToggleSidebar { get; set; }

        [Parameter]
        public bool isBodyToggleClass { get; set; }

        //--------------
        protected bool isLoading { get; set; } = true;
        private bool loaded = false;
        private string url = "core/Menu/ListAsync";
        protected string activeClass = null;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        protected string DefualtIcon(string ico,bool is_sub = false)
        {
            return ico.isNOEOW() ? (is_sub ? "fa fa-circle-thin" : "fa fa-circle-o") : ico;
        }
        protected virtual async Task LoadData()
        {
            if (!loaded)
            {
                isLoading = true;
                var response = await _api.Get($"{url}");
                if (response != null)
                {
                    if (response.Status == 200)
                    {
                        menuList = response.Data;
                    }
                    else
                        _toastService.ShowWarning(response.Message, "Failed");
                }
                else
                    _toastService.ShowError($"Fial to get menu", "Failed");
                loaded = true;                
            }  
            isLoading = false;
        }

    }
}
