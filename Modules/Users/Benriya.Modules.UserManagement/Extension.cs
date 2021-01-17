using Benriya.Core.Extensions;
using ExtCore.Infrastructure;

namespace Benriya.Modules.UserManagement
{
    public class Extension : ExtensionBase
    {
        public const string Policy = "UserManagement";
        public override string Name => "User managments";
        public override string Description => "User managements API using on Benriya.Core application";
        public override string Url => $"";
        public override string Helper => $"Permission policy format: {Policy}.x";
        public override string Code => Policy;
        public override string Version => "0.1 alpha";
        public override string Authors => "Tom, Sornarin";
    }
}
