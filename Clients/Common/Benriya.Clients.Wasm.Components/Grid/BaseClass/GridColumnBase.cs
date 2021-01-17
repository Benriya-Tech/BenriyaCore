using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace Benriya.Clients.Wasm.Components.Grid
{
    public class GridColumnBase : ComponentBase,IDisposable
    {
        [Inject]
        public IUrlManager _UrlManager { get; set; }
        [Inject]
        public NavigationManager _NavigationManager { get; set; }

        [Parameter]
        public string ColumnTitle { get; set; } = string.Empty;
        [Parameter]
        public string  Param { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnSearchTextChanged { get; set; }
        [Parameter]
        public bool SearchText { get; set; } = true;
        [Parameter]
        public string InputType { get; set; } = "text";
        
        protected string iniValue { get; set; }
        protected string SearchRenderText { get; set; }

        protected string SortBy { get; set; } = string.Empty;

        protected string SortOrder { get; set; } = string.Empty;
        protected int CountClicked { get; set; }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && !Param.isNOEOW())
            {
                iniValue = _UrlManager.GetQueryParm(Param);                
                SortBy = _UrlManager.GetQueryParm("sortby");
                SortOrder = _UrlManager.GetQueryParm("sortOrder");
            }else if(Param.isNOEOW())     
                iniValue = null;
        }

        protected void ClickedSort(string param,string sortBy)
        {
            CountClicked++;
            SortOrder = param == "asc" ? "desc" : "asc";
            SortBy = sortBy;
            _UrlManager.AddQueryParm("sortby",SortBy);
            _UrlManager.AddQueryParm("sortOrder",SortOrder);
            _UrlManager.AddQueryParm("page","1");
            _UrlManager.RemoveParm("pageSize");
            _NavigationManager.NavigateTo(_UrlManager.GetNavigate());
            Console.WriteLine("Sort order: "+SortOrder+" , Sort by: "+SortBy);
        }

        protected override void OnInitialized()
        {
            _NavigationManager.LocationChanged += LocationChanged;
        }
        private void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            //string navigationMethod = e.IsNavigationIntercepted ? "HTML" : "code";
            _UrlManager.Uri = e.Location;
            string sortby = _UrlManager.GetQueryParm("sortby");
            if (sortby.isNOEOW() || sortby != Param) {
                SortBy = string.Empty;
                SortOrder = string.Empty;
                StateHasChanged();
            }
            if (_NavigationManager.Uri.isNOEOW() || _NavigationManager.Uri.IndexOf("?") < 1 && _NavigationManager.Uri.IndexOf("&") < 1 || Param.isNOEOW())
            {
                SortBy = string.Empty;
                SortOrder = string.Empty;
                iniValue = null;
            }
        }

        public void Dispose()
        {
            _NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
