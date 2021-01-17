using Microsoft.AspNetCore.Components;

namespace Benriya.Clients.Wasm.Components.Modals
{
    public class ModalBase : ModalBaseMaster
    {
        [Parameter]
        public RenderFragment Body { get; set; }
        [Parameter]
        public RenderFragment Footer { get; set; }
        [Parameter]
        public RenderFragment ContentBtnCancel { get; set; }
    }
}
