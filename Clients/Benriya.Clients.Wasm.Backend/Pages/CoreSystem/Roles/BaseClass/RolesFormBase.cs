using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.SystemUsers;

namespace Benriya.Clients.Wasm.Backend.Pages.CoreSystem.Roles
{
    public class RolesFormBase : FormBase<User_Role>
    {
        public RolesFormBase()
        {
            model = new User_Role();
            url = "user/Roles/";
        }
    }
}
