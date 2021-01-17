using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.Menus;

namespace Benriya.Clients.Wasm.Backend.Pages.Menus
{
    public partial class BackendMenu_FormBase : FormBase<SystemMenu>
    {
        public BackendMenu_FormBase()
        {
            model = new SystemMenu();
            url = "core/Menu/";
        }
    }
}
