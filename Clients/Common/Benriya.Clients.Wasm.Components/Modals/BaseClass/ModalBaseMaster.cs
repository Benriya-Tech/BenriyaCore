using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Modals
{
    public class ModalBaseMaster : ComponentBase
    {
        [Parameter]
        public RenderFragment Title { get; set; }
        [Parameter]
        [StringLength(10)]
        public string Size { get; set; } = "lg";
        [Parameter]
        public string CssClassBtnCancel { get; set; } = "btn btn-secondary";
        [Parameter]
        public EventCallback<bool> OnClose { get; set; }
        [Parameter]
        [StringLength(10)]
        public string Position { get; set; } = "modal-dialog-centered";

        protected string modalDisplay = "none;";
        protected string modalClass = "";
        protected bool showBackdrop = false;

        public void Open()
        {
            modalDisplay = "block;";
            modalClass = "show";
            showBackdrop = true;
        }

        public async Task Close()
        {
            await OnClose.InvokeAsync(true);
            modalDisplay = "none";
            modalClass = "";
            showBackdrop = false;
        }
    }
}
