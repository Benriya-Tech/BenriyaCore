using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Classes
{
    public interface IClientMudule
    {
        string Name { get; }
        string Description  { get; }
        string Key  { get; }
        string Version  { get; }
        string[] Helper  { get; }
        string Authors { get; }
    }
}
