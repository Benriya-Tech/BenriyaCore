using ExtCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Benriya.Modules.Default.Classes
{
    public class IndexViewModel
    {
        public List<IExtension> exts { get; set; }
        public string OsPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        public string AspDotnetVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
        public string Runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        public string AppVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
    }
}
