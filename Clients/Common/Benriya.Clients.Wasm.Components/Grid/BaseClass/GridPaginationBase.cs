using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components.Loading.Component.Services;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Utils.Extensions;
using Benriya.Utils.Pagingation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Grid
{
    public class GridPaginationBase<T> : ComponentBase, IDisposable
    {
        [Inject]
        public NavigationManager _NavigationManager { get; set; }
        [Inject]
        public IUrlManager _UrlManager { get; set; }
        [Inject]
        public IApiClientService<T> _api { get; set; }
        [Inject]
        public ILoadingService _Loading { get; set; }

        /*----> Params <----*/
        [Parameter]
        public RenderFragment GridColumns { get; set; }

        [Parameter]
        public RenderFragment<T> GridRow { get; set; } = default;

        [Parameter]
        public NameValueCollection SearchColumns { get; set; }
        [Parameter]
        public GridSearchStyle SearchStyle { get; set; } = GridSearchStyle.ShowFirst;

        [Parameter]
        public List<T> Items { get; set; } = new List<T>();
        [Parameter]
        public string Url { get; set; }
        [Parameter]
        public int PageSize { get; set; } = PaginationConfig.page_size;

        [Parameter]
        public PagingHeader PagingHeader { get; set; }
        [Parameter]
        public bool ReloadList { get; set; }
        [Parameter]
        public bool isLoading { get; set; } = true;
        [Parameter]
        public string PageName { get; set; }
        [Parameter]
        public bool OnSearch { get; set; }
        

        /*----> Self var <----*/
        protected int CurrentPage { get; set; } = 1;
        protected string SearchRenderText { get; set; }
        protected List<int> Paging { get; set; }        
        private bool isFirstRender { get; set; } = true;
        private bool isSearchColumns { get; set; } = false;
        private List<T> _Items { get; set; } = new List<T>();
        
        private void UpdateUrl(int page)
        {
            bool isSearch = SearchColumns != null && SearchColumns.Count > 0;
            if (page == 1 && _NavigationManager.Uri.IndexOf("?") < 1 && !isSearch)
                return;            
            string sortOrder = _UrlManager.GetQueryParm("sortOrder");
            string sortBy = _UrlManager.GetQueryParm("sortby");
            _UrlManager.Reset();
            if (_UrlManager.Uri.isNOEOW())
                _UrlManager.Uri = PageName;// _UrlManager.GetNavigate(_NavigationManager.Uri);
            //Console.WriteLine($"Cleaned URL => {_NavigationManager.Uri}");
            //_UrlManager.RemoveParm("pageSize");
            _UrlManager.AddQueryParm("page", page.ToString(), true);
            if ((!sortOrder.isNOEOW() && !sortBy.isNOEOW()) && new[] { "asc", "desc" }.Contains(sortOrder))
            {
                _UrlManager.AddQueryParm("sortOrder", sortOrder);
                _UrlManager.AddQueryParm("sortby", sortBy);
            }

            if ( isSearch && !isFirstRender && !_NavigationManager.Uri.isNOEOW())
            {
                foreach (string k in SearchColumns)
                {
                    if (k != "page" && k != "pageSize") {
                        string v = SearchColumns[k];
                        if (k.isNOEOW() || v.isNOEOW())
                            continue;
                        _UrlManager.AddQueryParm(k, v);
                        v = null;
                    }
                }
            }
            else if (SearchColumns != null)
                SearchColumns.Clear();
            var uri = _UrlManager.GetNavigate(_NavigationManager.Uri);
            //var old = _UrlManager.GetNavigate(_NavigationManager.Uri);
            //Console.WriteLine($"Old => {_NavigationManager.Uri}");
            //Console.WriteLine($"New => {uri}");
            _NavigationManager.NavigateTo($"{uri}");
        }

        protected override void OnInitialized()
        {            
            _Loading.Show();
            _NavigationManager.LocationChanged += LocationChanged;
        }

        void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (_NavigationManager.Uri.isNOEOW() || _NavigationManager.Uri.IndexOf("?") < 1 || PageName.IndexOf("&") < 1)
            {
                _UrlManager.Uri = e.Location;
                if (_UrlManager.GetQueryParm("page").isNOEOW())
                    CurrentPage = 1;
                SearchColumns.Clear();
            }
            if (!PageName.isNOEOW() && _UrlManager.GetAbsolutePath(_NavigationManager.Uri) == _UrlManager.GetAbsolutePath(PageName))
                Task.Run(async () => await LoadData(e.Location));
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    Console.WriteLine("OnInitializedAsync()");
        //}

        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //}


        protected override async Task OnParametersSetAsync()
        {

            isLoading = false;
            if (OnSearch && SearchColumns != null && SearchColumns.Count > 0 && !isFirstRender)
            {
                isSearchColumns = true;
                UpdateUrl(1);
            }
            else if (OnSearch && isSearchColumns)
            {
                UpdateUrl(1);
                isSearchColumns = false;                
            }
            else
            {
                if ((Items == null || Items.Count() == 0) && _Items.Count() > 0)
                    Items = _Items;
                Console.WriteLine($"SetAsync:"+Items.Count());
            }
            OnSearch = false;
            await base.OnParametersSetAsync();
        }


        protected override bool ShouldRender()
        {
            //base.ShouldRender();
            return true;
        }
        protected override void OnAfterRender(bool firstRender)
        {
            isFirstRender = firstRender;
            if (!firstRender && ReloadList)
            {
                isLoading = true;
                string page = _UrlManager.GetQueryParm("page");
                int page_no = CurrentPage;
                if (!page.isNOEOW() && page.isNumberic())
                {
                    page_no = Int32.Parse(page);
                }
                else
                {
                    page_no = CurrentPage = 1;
                }
                Console.WriteLine($"Reload: {ReloadList}");
                UpdateList(page_no,true);
                ReloadList = false;
            }            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            isFirstRender = firstRender;
            if (firstRender)
            {
                await LoadData();
                StateHasChanged();
            }
            else
                ReloadList = false;
            _Loading.SetLoading(isLoading);
        }

        protected void UpdateList(int pageNumber = 0, bool firstRender = false,bool force = false)
        {             
            if (Items != null && PagingHeader != null)
            {
                bool chkPage = CurrentPage != pageNumber;
                var page_url = _UrlManager.GetQueryParm("page");
                if (!chkPage)
                    chkPage = (CurrentPage == 1 && pageNumber == 1) || (CurrentPage == PagingHeader.TotalPages && pageNumber == PagingHeader.TotalPages);
                if ((chkPage || page_url.isNOEOW() || force) && PagingHeader.TotalPages > 0)
                {
                    Paging = Pagination(pageNumber, PagingHeader.TotalPages);                    
                    CurrentPage = pageNumber;
                    string page = _UrlManager.GetQueryParm("page");
                    if (CurrentPage > 0 && CurrentPage <= PagingHeader.TotalPages && (page != null && page != CurrentPage.ToString()))
                    {
                        UpdateUrl(CurrentPage);
                    }
                }
                else
                    Console.WriteLine($"No update data: {chkPage}, CurrentPage={CurrentPage}, PageNumber={pageNumber}, Force:{force}");
            }
            else if (firstRender && pageNumber > 1)
            {
                CurrentPage = pageNumber;
            }
        }

        protected void NavigateTo(string direction)
        {
            bool force = false;
            if (direction == "prev" && CurrentPage > 1)
            {
                CurrentPage -= 1;
                force = true;
            }
            if (direction == "next")
            {
                if (CurrentPage < PagingHeader.TotalPages)
                {
                    CurrentPage += 1;
                    force = true;
                }
                else
                    CurrentPage = PagingHeader.TotalPages;
            }
            if (direction == "first")
                CurrentPage = 1;
            if (direction == "last")
                CurrentPage = PagingHeader.TotalPages;
            UpdateList(CurrentPage,false,force);

        }

        protected List<int> Pagination(int currentPage, int nrOfPages)
        {
            int delta = 2;
            var l = 0;
            var range = new List<int>();
            var rangeWith_x = new List<int>();
            range.Add(1);
            if (nrOfPages <= 1)            
                return range;            

            for (var i = currentPage - delta; i <= currentPage + delta; i++)
            {
                if (i < nrOfPages && i > 1)                
                    range.Add(i);                
            }
            range.Add(nrOfPages);
            foreach (var i in range)
            {
                if (l > 0)
                {
                    if (i - l == 2)                    
                        rangeWith_x.Add(l + 1);                    
                    else if (i - l != 1)                    
                        rangeWith_x.Add(0);                    
                }
                rangeWith_x.Add(i);
                l = i;
            }
            return rangeWith_x;
        }

        private async Task LoadData(string location = null)
        {            
            isLoading = true;
            StateHasChanged();
            await Task.Delay(100);
            var url = Url;
            _UrlManager.Uri = _NavigationManager.Uri;
            string page = _UrlManager.GetQueryParm("page");
            if (_UrlManager.Query.isNOEOW() || page.isNOEOW())
            {
                if (location.isNOEOW() && isFirstRender)
                    url = $"{url}?page=1&pageSize={PageSize}&req_s=1";
                else if (location.isNOEOW())
                    url = $"{url}?page=1&pageSize={PageSize}&req_s=2";
                else if (page.isNOEOW())
                    url = $"{url}?page=1&pageSize={PageSize}&req_s=3";
                else
                {
                    _UrlManager.AddQueryParm("pageSize", PageSize.ToString());
                    url = $"{url}{_UrlManager.GetQueryString(location)}";
                }
            }
            else
            {
                if (page.isNumberic())
                {
                    CurrentPage = int.Parse(page);
                    if (CurrentPage < 1) CurrentPage = 1;
                }
                _UrlManager.AddQueryParm("pageSize", PageSize.ToString());
                url = $"{url}{_UrlManager.GetQueryString()}";
            }
            try
            {
                var response = await _api.GetList(url);
                if (response == null || response.Data == null)
                {
                    Items = new List<T>();
                    PagingHeader = new PagingHeader();
                }
                else
                {
                    Items = _Items = response.Data;
                    PagingHeader = response.Paging;
                }
            } catch (Exception e)
            {
                PagingHeader = new PagingHeader();
                Items = new List<T>();
                Console.WriteLine(e.Message);
            }
            isLoading = false;
            StateHasChanged();

        }

        public void Dispose()
        {
            _NavigationManager.LocationChanged -= LocationChanged;
        }

        private bool collapseAdvanceSearch = true;

        protected string SearchRender{
            get
            {
                switch (SearchStyle)
                {
                    case GridSearchStyle.Hide: return "filter-hide";
                    case GridSearchStyle.Show: return "filter-show";
                    case GridSearchStyle.ShowFirst: return collapseAdvanceSearch ? "filter-show" : "filter-hide";
                    case GridSearchStyle.HideFirst: return collapseAdvanceSearch ? "filter-hide" : "filter-show";
                    default: return collapseAdvanceSearch ? "filter-show" : "filter-hide";
                }
            }
         }
        
        public void ToggleSearchRender()
        {            
            collapseAdvanceSearch = !collapseAdvanceSearch;
        }

    }
}
