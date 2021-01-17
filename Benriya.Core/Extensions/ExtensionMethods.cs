using System.Linq;
using System.Reflection;
using System.Web;

namespace Benriya.Core.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return string.Join("&", properties.ToArray());
        }

        public static string GetAssemblyLastname(this object obj)
        {
            var name = obj.GetType().Assembly.GetName().Name;
            return name.Substring(name.LastIndexOf(".")+1);
        }

    }
}
