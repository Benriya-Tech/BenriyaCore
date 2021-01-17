using System.Linq;
using System.Reflection;

namespace Benriya.Share.ViewModels
{
    public class ClaimPermission
    {
        public const string Read = "Read";        
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string Remove = "Remove";
        public const string Delete = "Delete";
        //----------
        public const string Info = "Info";
        public const string List = "List";

        public string[] GetPermissions() {
            var x = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            return x.Select(p => p.Name).ToArray();
            //return typeof(ClaimPermission).GetProperties().Select(p => p.Name).ToArray();
        }
    }
}
