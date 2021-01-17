using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Pages.CoreSystem.Policy
{
    public class PolicyFormBase : FormBase<Policy_Roles>
    {
        public PolicyFormBase()
        {
            model = new Policy_Roles();
            url = "core/Permission/";
        }    
    }
}
