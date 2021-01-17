using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
namespace Benriya.Utils
{
    public class NetworkInfo
    {

        public static string GetMacAdress()
        {
            return (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();
        }
        public static PhysicalAddress GetPhysicalAddress()
        {
            var InterfaceAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Select(n => n.GetPhysicalAddress())
                .FirstOrDefault();
            return InterfaceAddress;
        }
    }

    public class ClientInfomation
    {
        private HttpContext context;
        public HttpContext _httpContext {
            get => context;
            set => context = value ?? throw new ArgumentNullException(paramName:nameof(value),message:"Cannot set HttpContext");
        }
        public ClientInfomation(HttpContext httpContext) => (context) = (httpContext);
        public string GetClientIP()
        {
              return _httpContext != null ? _httpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress?.ToString() : "::1";
        }
        public  string GetConnectionID()
        {
            return  _httpContext?.Features.Get<IHttpConnectionFeature>().ConnectionId;
        }
        public string GetUserAgent()
        {
            if (_httpContext == null) return null;
            var userAgent = _httpContext.Request.Headers["User-Agent"];
            if (userAgent.Count() == 0)
                userAgent = _httpContext.Request.Headers["user-agent"];
            if (userAgent.Count() == 0)
                throw new HttpRequestException("Headers[User-Agent] not allowed");
            return userAgent.ToString();
        }

        public string GetClientID()
        {
            return _httpContext != null ? CryptographyCore.MD5_hash(GetClientIP() + GetUserAgent()) : null;
        }
    }

    public class UrlHelper 
    {
        public static string GetAbsolutePath(string uri)
        {
            var uriBuilder = new UriBuilder(uri);
            return uriBuilder.Uri.AbsolutePath;
        }

        public static string GetQueryParm(string parmName,string uri)
        {
            try
            {
                var uriBuilder = new UriBuilder(uri);
                var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                return q[parmName] ?? "";
            }
            catch (Exception) { return ""; }
        }

        public static string GetQueryString(string uri )
        {
            var uriBuilder = new UriBuilder(uri);
            return uriBuilder.Query;
        }

        public static string GetURL(string uri)
        {
            var uriBuilder = new UriBuilder(uri);
            return uriBuilder.ToString();
        }

        public static string GetNavigate(string uri)
        {
            return new Uri(GetURL(uri)).PathAndQuery;
        }

        public static string AddQueryParm(NameValueCollection KeyValue,string url)
        {
            var uriBuilder = new UriBuilder(url);
            uriBuilder.Query = KeyValue.ToString();
            return uriBuilder.ToString();
        }

        public static bool IsImageUrl(string URL)
        {
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(URL);
                request.KeepAlive = false;
                request.Method = "HEAD";
                using (var resp = request.GetResponse())
                {
                    return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                               .StartsWith("image/");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
