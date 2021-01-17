using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public interface IApiServiceProvider
    {
        Task IniClientInfo(bool reload = false);
        Task Reload();
    }
}