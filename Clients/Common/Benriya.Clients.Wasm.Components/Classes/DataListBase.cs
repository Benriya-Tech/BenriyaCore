using Benriya.Clients.Wasm.Components.Modals;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Classes
{
    public class DataListBase<T> : ComponentBase
    {
        [Inject]
        public NavigationManager _NavigationManager { get; set; }
        [Inject]
        public Toolbelt.Blazor.I18nText.I18nText i18nText { get; set; }
        protected I18nText.Manager _lang = new I18nText.Manager();
        protected bool OnSearch { get; set; } = false;
        protected Modal modal { get; set; }
        protected ModalContent modalForm { get; set; }
        protected T ModelData { get; set; } = Activator.CreateInstance<T>();
        protected bool isReset { get; set; }
        protected bool loadedFrm { get; set; } = true;
        protected NameValueCollection _SearchColumns;
        protected bool setStateChange { get; set; } = true;
        protected NameValueCollection SearchColumns
        {
            get
            {                
                if (_SearchColumns == null)
                {
                    var uri = new Uri(_NavigationManager.Uri);
                    _SearchColumns = System.Web.HttpUtility.ParseQueryString(uri.Query);
                }
                return _SearchColumns;
            }
            set { _SearchColumns = value; }
        }
        protected override async Task OnInitializedAsync()
        {            
            _lang = await i18nText.GetTextTableAsync<I18nText.Manager>(this);
        }
        protected virtual void OnSearchTextChanged(ChangeEventArgs changeEventArgs, string columnName)
        {
            string searchText = changeEventArgs.Value.ToString();
            SearchColumns.Remove(columnName);
            if (!searchText.isNOEOW())
                SearchColumns.Add(columnName, searchText);
            OnSearch = true;
        }
        protected virtual void HandleReset()
        {
            _NavigationManager.NavigateTo(UrlHelper.GetAbsolutePath(_NavigationManager.Uri));
            isReset = true;
        }

        protected virtual void ShowModal(T data)
        {
            loadedFrm = true;
            ModelData = data;
            modal.Open();
        }

        protected virtual void ShowForm(T data = default)
        {
            loadedFrm = false;
            ModelData = data == null ? Activator.CreateInstance<T>() : data;
            modalForm.Open();
        }

        protected virtual async Task OnActionCompleted(bool competed)
        {
            if (competed)
                await modalForm.Close();

        }
        protected virtual async Task OnCancelForm()
        {
            loadedFrm = true;
            await modalForm.Close();
        }
        protected virtual void OnCloseModal()
        {
            loadedFrm = true;
        }
        protected override void OnAfterRender(bool firstRender)
        {
            isReset = false;
        }

        protected virtual bool isNoQueryString()
        {
            return _NavigationManager.Uri.isNOEOW() || _NavigationManager.Uri.IndexOf("?") < 1 || _NavigationManager.Uri.IndexOf("&") < 1;
        }
    }
}