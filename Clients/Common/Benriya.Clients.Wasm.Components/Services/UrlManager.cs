using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Specialized;

namespace Benriya.Clients.Wasm.Components.Services
{
    public class UrlManager : IUrlManager
    {
        public string Uri { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public NameValueCollection KeyValue { get; set; }
        private readonly NavigationManager _navigationManager;
        public UriBuilder UriBuilder { get; set; }
        public UrlManager(NavigationManager nav)
        {
            _navigationManager = nav;
            Uri = _navigationManager.Uri;
            UriBuilder = new UriBuilder(Uri);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;

        }
        public void AddQueryParm(string parmName, string parmValue,bool reset = false)
        {
            if (reset)
            {
                if (KeyValue != null)
                {
                    KeyValue.Clear();
                    KeyValue[parmName] = parmValue;
                }
                else
                {
                    KeyValue = System.Web.HttpUtility.ParseQueryString($"?{parmName}={parmValue}");
                    KeyValue[parmName] = parmValue;
                }
            }else
            {
                if (KeyValue == null)
                    KeyValue = System.Web.HttpUtility.ParseQueryString(UriBuilder.Query);
                KeyValue[parmName] = parmValue;
            }
        }

        public string GetURL(string uri = null)
        {
            if(!uri.isNOEOW())
                UriBuilder = new UriBuilder(uri);

            UriBuilder.Query = KeyValue.ToString();
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
            return UriBuilder.ToString();
        }

        public string GetNavigate(string uri = null)
        {
            return new Uri(this.GetURL(uri)).PathAndQuery;
        }

        public void Reset()
        {
            KeyValue = System.Web.HttpUtility.ParseQueryString("");
            UriBuilder.Query = string.Empty;
            Uri = string.Empty;
            Query = string.Empty;
        }

        public void RemoveParm(string parmName)
        {
            if(KeyValue != null)
                KeyValue.Remove(parmName);
        }

       

        public string GetQueryString(string uri = null)
        {
            if (!uri.isNOEOW())
            {
                UriBuilder = new UriBuilder(uri);
                UriBuilder.Query = KeyValue.ToString();
                return UriBuilder.Query;
            }
            else
            {
                UriBuilder.Query = KeyValue.ToString();
                return UriBuilder.Query;
            }
        }

        public string GetAbsolutePath(string url = null,bool is_set = false)
        {
            if (!url.isNOEOW())
            {
                if (is_set) {
                    UriBuilder = new UriBuilder(url);
                    return UriBuilder.Uri.AbsolutePath;
                }
                else
                {
                    var uri = new UriBuilder(url);
                    return uri.Uri.AbsolutePath;
                }
            }
            else
                return UriBuilder.Uri.AbsolutePath;
        }

        // blazor: get query parm from url
        public string GetQueryParm(string parmName = "")
        {
            try
            {
                var uriBuilder = new UriBuilder(Uri);
                var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);            
                return q[parmName] ?? "";
            }
            catch (Exception) { return ""; }
        }
    }
}
