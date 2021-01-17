using System;
using System.Collections.Specialized;

namespace Benriya.Clients.Wasm.Components.Services
{
    public interface IUrlManager
    {
        NameValueCollection KeyValue { get; set; }
        string Path { get; set; }
        string Query { get; set; }
        string Uri { get; set; }
        UriBuilder UriBuilder { get; set; }

        void AddQueryParm(string parmName, string parmValue, bool reset = false);
        string GetQueryParm(string parmName);
        string GetURL(string uri = null);
        string GetQueryString(string uri = null);
        string GetAbsolutePath(string url, bool is_set = false);
        void RemoveParm(string parmName);
        string GetNavigate(string uri = null);
        void Reset();
    }
}