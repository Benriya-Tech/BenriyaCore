using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.SystemUsers;


namespace Benriya.Clients.Wasm.Backend.Pages.CoreSystem.Permission
{
    public class PermissionFormBase : FormBase<Permission_Access>
    { 
        public PermissionFormBase()
        {
            model = new Permission_Access();
            url = "core/Permission/";
        }
    }
}
