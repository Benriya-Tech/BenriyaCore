using Benriya.Clients.Wasm.Components.Loading.Component.Services;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Blazored.TextEditor;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Clients.Wasm.Components.Classes;

namespace Benriya.Clients.Wasm.Components.Forms
{
    public class FormBase<T> : ComponentBase
    {
        [Inject]
        public IApiClientService<T> _api { get; set; }
        [Inject]
        public IToastService _toastService { get; set; }
        [Inject]
        public NavigationManager _NavigationManager { get; set; }
        [Inject]
        public ILoadingService _Loading { get; set; }
        [Inject]
        public Toolbelt.Blazor.I18nText.I18nText i18nText { get; set; }
        
        protected I18nText.Manager _lang = new I18nText.Manager();

        [Parameter]
        public string id { get; set; }
        [Parameter]
        public string Redirect { get; set; }
        [Parameter]
        public string url { get; set; }
        [Parameter]
        public string save_url { get; set; }
        [Parameter]
        public string ActionRedicrect { get; set; }
        [Parameter]
        public RenderFragment RenderForm { get; set; }
        [Parameter]
        public T model { get; set; } = Activator.CreateInstance<T>();
        [Parameter]
        public bool loaded { get; set; } = false;
        [Parameter]
        public bool isReload { get; set; } = false;
        [Parameter]
        public string TextEditorField { get; set; }
        [Parameter]
        public EventCallback<bool> OnActionCompleted { get; set; }
        [Parameter]
        public EventCallback<T> OnDataLoaded { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnCancel { get; set; }
        [Parameter]
        public DisplayType DisplayFormType { get; set; } = DisplayType.Page;
        public FormServValidator formValidator { get; set; }
        public Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public bool has_id { get; set; } = false;
        public bool isLoading { get; set; }     
        public BlazoredTextEditor QuillHtml;
        public bool disabled { get; set; }
        protected string current_id { get; set; }
        private bool renderHtml = false;
        private bool isSaved = false;
        protected override async Task OnInitializedAsync()
        {
            _lang = await i18nText.GetTextTableAsync<I18nText.Manager>(this);
            await LoadData();
        }

        protected override async Task OnParametersSetAsync()
        {
            has_id = !id.isNOEOW() && (!id.isNumberic() || !id.IsGuid()) && id != "0" && id != Guid.Empty.ToString();
            //Console.WriteLine($"+++++++++++++++++++ form id: {id} , has id: {has_id}");
            if (!loaded)            
                await LoadData();            
        }

        protected virtual async Task LoadData()
        {            
            if (!loaded && has_id)
            {
                isLoading = true;
                _Loading.Show();
                var response = await _api.Get($"{url}{id}");
                if (response != null)
                {
                    if (response.Status == 200)
                    {
                        current_id = id;
                        model = response.Data;
                        await OnDataLoaded.InvokeAsync(model);
                    }
                    else
                        _toastService.ShowWarning(response.Message, "Failed");
                }
                else                                    
                    _toastService.ShowError($"Fial to get: {id}", "Failed");
                
                
                loaded = true;
                isLoading = false;
                _Loading.Hide();
            }
            else if(id == "0" || id == Guid.Empty.ToString())
            {
                if((!has_id || model == null) && !loaded)
                    model = Activator.CreateInstance<T>();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await LoadContent();            
        }       

        private async Task LoadContent(bool is_force = false)
        {
            if (!TextEditorField.isNOEOW() && !isLoading && model != null && (is_force || (!renderHtml && has_id)))
            {
                try
                {
                    Type type = model.GetType();
                    PropertyInfo prop = type.GetProperty(TextEditorField);
                    await Task.Delay(300);
                    await QuillHtml.LoadHTMLContent(prop.GetValue(model).ToString());
                    renderHtml = true;
                }
                catch (Exception) { Console.WriteLine("Cannot convert from text editor data"); }
            }
        }

        protected virtual bool TrySetProperty(string property, object value)
        {
            var prop = model.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(model, value, null);
                return true;
            }
            return false;
        }

        protected virtual async Task HandleValidSubmit()
        {
            if(formValidator != null)
                formValidator.ClearErrors();
            disabled = true;
            isSaved = false;
            string msg = "Create successful";
            ApiResultModel<T> response = null;
            isLoading = true;
            _Loading.Show();
            bool is_action = false;

            if (!TextEditorField.isNOEOW())
            {
                var value = await QuillHtml.GetHTML();
                if(TrySetProperty(TextEditorField, value))
                {
                    Console.WriteLine(TextEditorField+" has value...");
                }
            }

            if (id.isNOEOW() || (id == "0" || id == Guid.Empty.ToString()))
            {
                is_action = true;
                response = await _api.Post($"{(save_url.isNOEOW() ? url : save_url)}", model);
            }
            else if(has_id)
            {
                is_action = true;
                response = await _api.Update($"{(save_url.isNOEOW() ? url : save_url)}{id}", model);
                msg = "Update successful";
            }
            if(is_action)
            {
                if (response != null)
                {
                    if (response.Status == 200 && response.Data != null)
                    {
                        model = response.Data;
                        _toastService.ShowSuccess(msg, "Completed");
                        await OnActionCompleted.InvokeAsync(true);
                        if (isReload)
                            _NavigationManager.NavigateTo(_NavigationManager.Uri);
                        isSaved = true;
                    }
                    else
                    {
                        if (formValidator != null && response.Errors != null && response.Errors.Count > 0)                        
                            formValidator.DisplayErrors(response.Errors);                        
                        _toastService.ShowWarning(response.Message, response.Title);                        
                    }                    
                }
                else
                {
                    _toastService.ShowError("Cannot save data", "Fail");
                }
            }
            else
            {
                _toastService.ShowError("Please check data format", "Fail");
            }
            isLoading = false;
            _Loading.Hide();
            if (isSaved)
            {
                if (Redirect.isNOEOW())
                    await LoadContent(is_action);
                else
                    _NavigationManager.NavigateTo(Redirect);
            }
            disabled = false;
        }

        public void ResetModel()
        {
            if (formValidator != null)
                formValidator.ClearErrors();
            model = Activator.CreateInstance<T>();
        }
        
        public virtual async Task InsertImageUpload(string url)
        {
            Console.WriteLine(url);
            await this.QuillHtml.InsertImage(url);
        }
    }
}
