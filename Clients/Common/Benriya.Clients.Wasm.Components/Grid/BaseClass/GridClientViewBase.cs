using Benriya.Clients.Wasm.Components.Classes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benriya.Clients.Wasm.Components.Grid
{
    public class GridClientViewBase<TItem> : ComponentBase
    {
        [Parameter]
        public RenderFragment GridColumns { get; set; }

        [Parameter]
        public RenderFragment<TItem> GridRow { get; set; }

        [Parameter]
        public List<TItem> Items { get; set; }
        [Parameter]
        public int Total { get; set; }
        

        public List<TItem> ItemList { get; set; }

        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        [Parameter]
        public bool ReloadList { get; set; }
        [Parameter]
        public GridSearchStyle SearchStyle { get; set; } = GridSearchStyle.ShowFirst;


        protected override void OnInitialized()
        {
            PageSize = 5;

            if (Items != null)
            {
                ItemList = Items.Take(PageSize).ToList();
                TotalPages = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);
            }

            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender && ReloadList)
            {
                UpdateList();
                ReloadList = false;

                StateHasChanged();
            }

            base.OnAfterRender(firstRender);
        }

        protected void UpdateList(int pageNumber = 0)
        {
            if (Items != null)
            {
                //pageNumber * pageSize -> take 5
                ItemList = Items.Skip(pageNumber * PageSize).Take(PageSize).ToList();
                TotalPages = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);
                CurrentPage = pageNumber;
            }
        }

        protected void NavigateTo(string direction)
        {
            if (direction == "prev" && CurrentPage != 0)
                CurrentPage -= 1;
            if (direction == "next" && CurrentPage != TotalPages - 1)
                CurrentPage += 1;
            if (direction == "first")
                CurrentPage = 0;
            if (direction == "last")
                CurrentPage = TotalPages - 1;

            UpdateList(CurrentPage);
        }

        private bool collapseAdvanceSearch = true;

        protected string SearchRender
        {
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
