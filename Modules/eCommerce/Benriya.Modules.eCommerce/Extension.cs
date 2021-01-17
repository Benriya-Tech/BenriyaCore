using ExtCore.Infrastructure;
using Benriya.Core.Extensions;

namespace Benriya.Modules.eCommerce
{
    

    public class Extension : ExtensionBase
    {
        public const string Policy = "eCommerce";
        public override string Name => "E-commerce servics";
        public override string Description => "E-commerce API module using on Benriya.Core applications";
        public override string Url => $"Permission plicy format: {this.GetAssemblyLastname()}.x";
        public override string Helper => $"Permission plicy format: {Policy}.x";
        public override string Version => "0.1 alpha";
        public override string Code => Policy;
        public override string Authors => "xxxxxxxxxxxxxxxxxxxxxxxx";
    }
}