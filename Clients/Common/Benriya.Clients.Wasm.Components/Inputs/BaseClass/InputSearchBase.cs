using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Inputs
{
    public class InputSearchBase<T> : ComponentBase
    {
        [Inject]
        public IApiClientService<T> _api { get; set; }

        [Parameter]
        public string URL { get; set; }
        [Parameter]
        public string SeatchParam { get; set; } = "q";

        [Parameter]
        public int Limit { get; set; } = 10;
        [Parameter]
        public EventCallback<IEnumerable<T>> OnSearchData { get; set; }
        [Parameter]
        public bool AutoLoad { get; set; }
        [Parameter]
        public EventCallback<bool> OnLoading { get; set; }
        private string current_url { get; set; }
        private string searchText;
        private bool isLoaded { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if(AutoLoad)
                await LoadData(null);
            isLoaded = true;
        }

        protected async Task OnSearchTextChanged(ChangeEventArgs e)
        {
            await LoadData(e.Value.ToString());
        }

        protected virtual async Task LoadData(string q)
        {
            searchText = q;
            current_url = URL;
            await OnLoading.InvokeAsync(true);
            var response = await _api.GetCustom<IEnumerable<T>>($"{URL}{(URL.IndexOf('?') > 0 ? "&" : "?")}limit={Limit}&q={q}");
            if (response.Status == 200 && response.Data != null)
                await OnSearchData.InvokeAsync(response.Data);
            else
                await OnSearchData.InvokeAsync(new List<T>());
            await OnLoading.InvokeAsync(false);
            
        }

        protected override async Task OnParametersSetAsync()
        {            
            if (isLoaded && current_url != URL)
            {
                Console.WriteLine("------------- current_url: " + current_url);
                Console.WriteLine("------------- URL: " + URL);
                await LoadData(searchText);                
            }
        }
    }
}
