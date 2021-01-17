using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Share.Models.FileStore;
using Benriya.Share.Models.Menus;
using Benriya.Share.SearchModels;
using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Pages.Filestore
{
    public partial class FileStoreType_ListBase : DataListBase<FileStore_FileType>, IDisposable
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

        protected override async Task OnInitializedAsync()
        {
            //_header.setTitle("Menu data");
            customSearch.user_name = SearchColumns.Get(nameof(customSearch.user_name));
            await base.OnInitializedAsync();
        }
        protected virtual async void SetLang(string langCode)
        {
            await i18nText.SetCurrentLanguageAsync(langCode);
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
