using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Clients.Wasm.Components.Modals;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Utils;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory.Warehouses
{
    public class WarwhouseAreaFormBase : ComponentBase
    {
        [Inject]
        public IApiClientService<Warehouse_Area> _api { get; set; }
        [Inject]
        public IToastService _toastService { get; set; }
        [Parameter]
        public Warehouse_Area ModelData { get; set; } = new Warehouse_Area();
        [Parameter]
        public EventCallback<Warehouse_Area> OnSubmit { get; set; }
        [Parameter]
        public EventCallback<Warehouse_Area> OnReset { get; set; }
        [Parameter]
        public EventCallback<Warehouse_Area> OnCancel { get; set; }
        [Parameter]
        public bool isNew { get; set; }
        private Warehouse_Area currentModelData;
        protected FormServValidator formValidator { get; set; }
        protected override void OnInitialized()
        {
            currentModelData = new Warehouse_Area()
            {
                id = ModelData.id,
                name = ModelData.name,
                description = ModelData.description,
                created = ModelData.created,
                updated = ModelData.updated
            };
        }
        protected async void HandleValidSubmitModal()
        {
            ApiResultModel<Warehouse_Area> response = await _api.Post($"inventory/Warehouse/CheckArea", ModelData);
            if (response.Status == 200)
            {
                await OnSubmit.InvokeAsync(ModelData);
            }
            else
            {
                if (formValidator != null && response.Errors != null && response.Errors.Count > 0)
                    formValidator.DisplayErrors(response.Errors);
                _toastService.ShowWarning(response.Message, "Failed");
            }
        }

        protected async void OnResetModel()
        {
            formValidator.ClearErrors();
            ModelData = currentModelData;
           await OnReset.InvokeAsync(currentModelData);
        }
        protected async Task OnCancelModel()
        {
            formValidator.ClearErrors();
            ModelData = currentModelData;
            await OnCancel.InvokeAsync(currentModelData);
        }
    }
}
