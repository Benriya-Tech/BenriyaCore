using Benriya.Clients.Wasm.Components.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend
{
    public class ClientMudule : IClientMudule
    {
        public string Name => "Core system";

        public string Description => "Core system UI";

        public string Key => "2b90db79-bb5338f9-d7328bee-adcea4eb";

        public string Version => "0.1 beta";

        public string[] Helper => new string[]
        {
            "Menus",
            "1. /admin/backendmenu",
            "Policy/Permission",
            "1. /admin/permission",
            "2. /admin/policy",
            "Users management",
            "1. /admin/sysroles",
            "2. /admin/sysusers",
            "Modules",
            "1. /admin/modules",
            "2. /admin/mod_frontend",
            "Tags",
            "1. /admin/tags",
            "2. /admin//tagsgroup",

        };

        public string Authors => "";
    }
}
