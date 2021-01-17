using Benriya.Clients.Wasm.Components.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory
{
    public class ClientMudule : IClientMudule
    {
        public string Name => "Inventory";

        public string Description => "Inventory UI";

        public string Key => "2b90db79-bb5338f9-d7328bee-adcea4eb";

        public string Version => "0.1 beta";

        public string[] Helper => new string[]
        {
            "Inventory URL",
            "1. /admin/inventory/wh",
            "2. Goods",
            " - /admin/inventory/goods",            
            " - /admin/inventory/category",
            " - /admin/inventory/goodsunit",
        };

        public string Authors => "";
    }
}
