using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Share.SearchModels;
using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory.Warehouses
{
    public class WarehouseListBase : DataListBase<Warehouse>, IDisposable
    {
        protected Search_User_Role customSearch { get; set; } = new Search_User_Role();

        protected void HandleValidSubmitSearch()
        {
            string user_name = nameof(customSearch.user_name);
            SearchColumns.Remove(user_name);
            if (!customSearch.user_name.isNOEOW())
                SearchColumns.Add(user_name, customSearch.user_name);
            OnSearch = true;
        }
        protected override void ShowForm(Warehouse data = null)
        {
            _NavigationManager.NavigateTo($"/admin/inventory/whform/{(data != null ? data.id.ToString() : null)}");
        }

        protected override async Task OnInitializedAsync()
        {
            customSearch.user_name = SearchColumns.Get(nameof(customSearch.user_name));
            await base.OnInitializedAsync();
        }

        protected override void OnInitialized()
        {
            _NavigationManager.LocationChanged += LocationChanged;
        }
        protected void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (setStateChange && this.isNoQueryString())
            {
                setStateChange = false;
                SearchColumns.Clear();
                customSearch.user_name = null;
                StateHasChanged();
                Console.WriteLine("Cleared");
            }
            else
                setStateChange = true;
            OnSearch = false;
        }

        public void Dispose()
        {
            _NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
