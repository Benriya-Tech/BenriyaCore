using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Benriya.Clients.Wasm.Components.Modals
{
    public class ModalConfirmBase : ModalBaseMaster
    {
        [Parameter]
        public RenderFragment Body { get; set; }
        [Parameter]
        public RenderFragment Footer { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnConfirm { get; set; }
        [Parameter]
        public string CssClassBtnOk { get; set; } = "btn btn-primary";
        [Parameter]
        public RenderFragment ContentBtnCancel { get; set; }
        [Parameter]
        public RenderFragment ContentBtnOk { get; set; }

        public ModalConfirmBase()
        {
            Size = "md";
        }
    }
}
