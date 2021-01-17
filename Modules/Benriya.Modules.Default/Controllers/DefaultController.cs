using Benriya.Modules.Default.Classes;
using Benriya.Utils;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Benriya.Modules.Default.Controllers
{
    public class DefaultController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<DefaultController> _logger;
        public DefaultController(ILogger<DefaultController> logger,IOptions<AppSettings> appSetting)
        {
            _appSettings = appSetting.Value;
            _logger = logger;
        }
        public ActionResult Index()
        {
            List<IExtension> exts;
            if (_appSettings.AssemblyCandidate)
            {
                string[] starts = new string[] { "ExtCore" };
                exts = ExtensionManager.GetInstances<IExtension>().Where(x => !starts.Any(sl => x.Name.StartsWith(sl))).ToList();
            }
            else
            {
                exts = ExtensionManager.GetInstances<IExtension>().ToList();
            }

            return this.View(new IndexViewModel() { exts = exts});
        }
    }
}