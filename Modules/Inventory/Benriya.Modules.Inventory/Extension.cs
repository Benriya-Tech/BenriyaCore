using ExtCore.Infrastructure;
using Benriya.Core.Extensions;

namespace Benriya.Modules.Inventory
{
    

    public class Extension : ExtensionBase
    {
        public const string Policy = "Inventory";
        public override string Name => "Inventory servics";
        public override string Description => "Inventory API module using on Benriya.Core applications";
        public override string Url => $"Permission plicy format: {this.GetAssemblyLastname()}.x";
        public override string Helper => $"Permission plicy format: {Policy}.x";
        public override string Version => "0.1 alpha";
        public override string Code => Policy;
        public override string Authors => "xxxxxxxxxxxxxxxxxxxxxxxx";
    }
}