using Benriya.Clients.Wasm.Components.Modals;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Actions
{
    public class DeleteButtonBase :ComponentBase
    {
        [Inject]
        public IApiClientService<bool> _api { get; set; }
        [Inject]
        public IToastService _toastService { get; set; }
        [Inject]
        public NavigationManager _NavigationManager { get; set; }

        /*---- Params ----*/
        [Parameter]
        public string id { get; set; }

        [Parameter]
        public string url { get; set; }

        [Parameter]
        public bool isReload { get; set; }

        [Parameter]
        public string Redirect { get; set; }
        [Parameter]
        public RenderFragment Content { get; set; }
        [Parameter]
        public RenderFragment BtnContent { get; set; }
        [Parameter]
        public RenderFragment Title { get; set; }
        [Parameter]
        [StringLength(50)]
        public string CssClass { get; set; }
        [Parameter]
        [StringLength(10)]
        public string BtnSize { get; set; }
        [Parameter]
        public EventCallback<bool> OnActionCompleted { get; set; }
        public bool isLoading { get; set; }
        protected ModalConfirm modal { get; set; }


        protected virtual void OnClickDelete()
        {
            modal.Open();
        }
        protected virtual async Task ConfirmDelete()
        {
            url = url.Trim();
            if (!url.EndsWith("/") && !url.EndsWith("="))
                url = url + "/";
            await HandleDelete();
        }

        protected virtual async Task HandleDelete()
        {
            if (!id.isNOEOW() && (!id.isNumberic() || !id.IsGuid()))
            {
                isLoading = true;
                var response = await _api.Delete($"{url}{id}");
                if (response != null)
                {
                    if (response.Status == 200 && response.Data)
                    {
                        _toastService.ShowSuccess("The data was deleted", "Completed");
                        await OnActionCompleted.InvokeAsync(true);
                        if(!Redirect.isNOEOW())
                            _NavigationManager.NavigateTo(Redirect);
                        else if (isReload)
                            _NavigationManager.NavigateTo(UrlHelper.GetAbsolutePath(_NavigationManager.Uri));
                        await modal.Close();
                    }
                    else
                    {
                        _toastService.ShowWarning(response.Message, "Failed");
                        await OnActionCompleted.InvokeAsync(false);
                    }
                }
                else
                {
                    _toastService.ShowError($"Fial to remove: {id}", "Failed");
                    await OnActionCompleted.InvokeAsync(false);
                }
                isLoading = false;
            }
        }
    }
}
