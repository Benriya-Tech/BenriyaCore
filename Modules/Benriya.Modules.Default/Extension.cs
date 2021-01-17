using Benriya.Core.Extensions;
using ExtCore.Infrastructure;

namespace Benriya.Modules.Default
{
    public class Extension : ExtensionBase
    {
        public const string Policy = "Default";
        public override string Name => "MVC default app";
        public override string Description => "MVC module using on Benriya.Core application";
        //public override string Url => "";
        public override string Helper => $"";
        public override string Code => Policy;
        public override string Version => "0.1";
        public override string Authors => "Tom, Sornarin";
    }
}