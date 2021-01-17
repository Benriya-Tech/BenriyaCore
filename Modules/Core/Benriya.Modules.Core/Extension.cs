using ExtCore.Infrastructure;
using Benriya.Core;
using Benriya.Core.Extensions;

namespace Benriya.Modules.Core
{
    

    public class Extension : ExtensionBase
    {
        public const string Policy = "Core";
        public override string Name => "Core servics";
        public override string Description => "Benriya Main API module using on Benriya.Core applications";
        public override string Url => $"";
        public override string Helper => $"Permission plicy format: {Policy}.x";
        public override string Code => Policy;
        public override string Version => "0.1 alpha";
        public override string Authors => "xxxxxxxxxxxxxxxxxxxxxxxx";
    }
}
