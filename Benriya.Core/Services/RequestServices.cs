using Benriya.Core.Abstractions;
using Benriya.Share.Abstractions;
using Benriya.Share.Models;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
//using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Benriya.Core.Services
{
    //[ScopedService]
    public class RequestServices : IRequestServices
    {
        public int id { get; set; }
        public Guid uuid { get; set; }
        public CurrentUser CurrentUser { get; set; } = new CurrentUser();
        public ClientInfo Info { get; set; } = new ClientInfo();
        public AppSettings ServerSettings { get; private set; }
        public IServiceProvider serviceProvider { get; private set; }
        public RequestServices(IOptions<AppSettings> appSetting,IServiceProvider svp)
        {
            ServerSettings = appSetting.Value;
            serviceProvider = svp;
        }

        public void SetUser(CurrentUser idata)
        {
            CurrentUser = idata == null ? new CurrentUser() : idata;

        }

        public void SetClientInfo(ClientInfo idata)
        {
            Info = idata == null ? new ClientInfo() : idata;
        }
    }

}
