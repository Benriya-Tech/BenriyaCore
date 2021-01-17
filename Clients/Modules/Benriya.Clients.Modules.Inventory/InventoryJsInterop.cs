using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class InventoryJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public InventoryJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Benriya.Clients.Modules.Inventory/InventoryJsInterop.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
