using System;
using System.Collections.Generic;
using System.Text;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class MetaDataProvider
    {
        public string Title = "Backend: Benriya Clients";

        public Action CallStateHasChange;


        public void setTitle(string title)
        {
            Title = title;
            CallStateHasChange();
        }
    }
}
