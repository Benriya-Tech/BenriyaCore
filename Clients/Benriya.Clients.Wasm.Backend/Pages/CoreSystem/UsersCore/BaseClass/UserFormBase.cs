using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.ViewModels;


namespace Benriya.Clients.Wasm.Backend.Pages.CoreSystem.UsersCore
{
    public class UserFormBase : FormBase<UsersEditModel>
    {
        public UserFormBase()
        {
            model = new UsersEditModel();
            url = "user/Users/ToEdit/";
            save_url = url = "user/Users/";
        }
    }
}
