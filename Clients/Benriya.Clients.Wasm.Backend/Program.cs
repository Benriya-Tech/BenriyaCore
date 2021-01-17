using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.Toast;
using Benriya.Clients.Wasm.Components.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Tewr.Blazor.FileReader;
using Benriya.Share.ViewModels;
using System.Net.Http.Json;
using Benriya.Clients.Wasm.Components.Models;
using Benriya.Utils;
using System.Security.Claims;
using Benriya.Clients.Wasm.Components.Loading.Component.Services;
using System.Reflection;
using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components;

namespace Benriya.Clients.Wasm.Backend
{
    public class Program
    {
        public static List<Assembly> ModulesRegistered  = new List<Assembly>();
        public static List<IClientMudule> ModulesInstalled  = new List<IClientMudule>();
        private static List<Type> Modules = new List<Type>();
        public static async Task Main(string[] args)
        {            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);//.UseHeadElementServerPrerendering();;
            builder.RootComponents.Add<App>("app");

            

            //-----------------------------| Register extensions |---------------------------------------
            Modules.Add(typeof(Benriya.Clients.Modules.CMS.ClientMudule));
            Modules.Add(typeof(Benriya.Clients.Modules.Inventory.ClientMudule));
            Modules.Add(typeof(Benriya.Clients.Modules.eCommerce.ClientMudule));






            //builder.RootComponents.Add<MetaData>("head");   
            //builder.Services.AddBlazorise(options =>
            // {
            //     options.ChangeTextOnKeyPress = true;
            // }).AddBootstrapProviders().AddFontAwesomeIcons();

            //builder.Services.AddRazorComponentsRuntimeCompilation();
            //builder.Services.AddScoped<MetaDataProvider>();
            #region Register extensions            
            //------------------------------- start load extensions
            ModulesSetup();
            //---------------------------------------------------------------------
            #endregion End of load extensions         
            var uri = new Uri(CLIENT_CONFIG.API_URL);
            builder.Services.AddScoped<IUrlManager, UrlManager>();
            builder.Services.AddScoped<IAuthenServices, AuthenServices>();            
            builder.Services.AddScoped(typeof(IApiClientService<>), typeof(ApiClientService<>));
            builder.Services.AddBlazoredToast();
            builder.Services.AddHeadElementHelper();
            builder.Services.AddBlazoredLocalStorage(config =>config.JsonSerializerOptions.WriteIndented = true);
            builder.Services.AddBlazoredSessionStorage(config =>config.JsonSerializerOptions.WriteIndented = true);
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IApiServiceProvider, ApiServiceProvider>();            
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = uri });// builder.HostEnvironment.BaseAddress) });
            builder.Services.AddI18nText();
            builder.Services.AddLoadingComponent();
            builder.Services.AddFileReaderService(o=> { o.UseWasmSharedBuffer = true; });
            
            var client = new HttpClient { BaseAddress = uri };
            builder.Services.AddAuthorizationCore(async options=>
            {
                var perms = new ClaimPermission().GetPermissions();
                var client = new HttpClient { BaseAddress = uri };
                var response = await client.GetFromJsonAsync<ApiResultModel<List<ExtensionModel>>>($"core/Modules");
                //var policies = new Dictionary<string,string>();
                if (response != null && response.Status == 200 && response.Data != null)
                {
                    foreach (var ext in response.Data)
                    {
                        foreach (var perm in perms)
                        {                            
                            var policy_name = $"{ext.Code}.{perm}";
                            //policies.Add(policy_name,policy_name);
                            options.AddPolicy(policy_name, policy => policy.RequireClaim(ClaimTypes.Role, policy_name));
                        }
                    }
                }
            });
            
            // builder
            //---------------------------------            
            var host = builder.Build();

            var response = await client.GetFromJsonAsync<ApiResultModel<string>>($"core/Sys/Version");
            if (response != null && response.Status == 200)
            {
                var sessionStorage = host.Services.GetRequiredService<ISessionStorageService>();
                await sessionStorage.SetItemAsync<string>(AppClient.VersionKey,response.Data);
                sessionStorage = null;
            }

            //---------------------------------
            //host.Services.UseBootstrapProviders().UseFontAwesomeIcons();   
            //if(policies)
            //var sessionStorage = host.Services.GetRequiredService<ISessionStorageService>();

            //await builder.Build().RunAsync();
            await host.RunAsync();
        }


        private static void ModulesSetup()
        {
            var core_mod = new ClientMudule();
            ModulesInstalled.Add(core_mod);
            foreach(var t in Modules)
            {
                var mod = (IClientMudule)Activator.CreateInstance(t);
                if (mod.Key.Equals(core_mod.Key))
                {
                    ModulesInstalled.Add(mod);
                    ModulesRegistered.Add(t.Assembly);
                }
            }
            core_mod = null;
        }
    }
}
