using Benriya.Clients.Wasm.Components.Loading.Component.Services;
using Benriya.Clients.Wasm.Components.Models;
using Benriya.Clients.Wasm.Components.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Pages.Modules
{
    public class InstalledModuleBase : ComponentBase
    {

        [Inject]
        public IApiClientService<List<ExtensionModel>> _api { get; set; }
        [Inject]
        public ILoadingService _Loading { get; set; }
        protected List<ExtensionModel> model {get;set;}
        protected bool isLoading { get; set; }

        private bool loaded = false;

        protected override async Task OnParametersSetAsync()
        {

            if (!loaded)
                await LoadData();

        }

        protected virtual async Task LoadData()
        {
            if (!loaded)
            {
                isLoading = true;
                _Loading.Show();
                var response = await _api.Get($"core/Modules");
                if (response != null)
                {
                    if (response.Status == 200 && response.Data != null)
                    {
                        model = response.Data;
                    }
                }                
            }
            loaded = true;
            isLoading = false;
            _Loading.Hide();
        }
    }
}
