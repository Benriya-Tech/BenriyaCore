using ExtCore.Infrastructure;

namespace Benriya.Modules.FileStore
{


    public class Extension : ExtensionBase
    {
        public const string Policy = "FileStore";
        public override string Name => "File store servics";
        public override string Description => "Benriya Main API module using on Benriya.Core applications";
        public override string Url => $"";
        public override string Helper => $"Permission plicy format: {Policy}.x";
        public override string Code => Policy;
        public override string Version => "0.1 alpha";
        public override string Authors => "xxxxxxxxxxxxxxxxxxxxxxxx";
    }
}
