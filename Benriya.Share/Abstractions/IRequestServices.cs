using Benriya.Share.Models;
using Benriya.Utils;
using Benriya.Utils.Models;
using System;

namespace Benriya.Share.Abstractions
{
    public interface IRequestServices
    {
        ClientInfo Info { get; set; }
        CurrentUser CurrentUser { get; set; }
        int id { get; set; }
        Guid uuid { get; set; }
        AppSettings ServerSettings { get; }
        IServiceProvider serviceProvider { get; }
        void SetClientInfo(ClientInfo idata);
        void SetUser(CurrentUser idata);
    }
}