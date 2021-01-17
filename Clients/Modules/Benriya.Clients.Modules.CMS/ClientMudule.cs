using Benriya.Clients.Wasm.Components.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.CMS
{
    public class ClientMudule : IClientMudule
    {
        public const string AdminPath = "admin/cms";
        public const string PublicPath = "cms";
        public string Name => "CMS";

        public string Description => "CMS client UI";

        public string Key => "2b90db79-bb5338f9-d7328bee-adcea4eb";

        public string Version => "0.1 beta";

        public string[] Helper => new string[]
        {
            "Backend URL",
            "1. /cms/category",
            "2. /cms/contents",
            "Frontend URL",
            "1. /cms",
            "2. /cms/read"
        };

        public string Authors => "";
    }
}
