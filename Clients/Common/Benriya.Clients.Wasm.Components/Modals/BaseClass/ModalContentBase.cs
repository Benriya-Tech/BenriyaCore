using Microsoft.AspNetCore.Components;

namespace Benriya.Clients.Wasm.Components.Modals
{
    public class ModalContentBase : ModalBaseMaster
    {
        [Parameter]
        public RenderFragment Content { get; set; }
    }
}
