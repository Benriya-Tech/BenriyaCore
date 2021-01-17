using Benriya.Clients.Wasm.Components.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.eCommerce
{
    public class ClientMudule : IClientMudule
    {
        public string Name => "E-Commerce";

        public string Description => "E-Commerce modules";

        public string Key => "2b90db79-bb5338f9-d7328bee-adcea4eb";

        public string Version => "0.1 beta";

        public string[] Helper => null;

        public string Authors => null;
    }
}
