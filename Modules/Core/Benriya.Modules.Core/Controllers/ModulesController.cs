using Benriya.Core;
using Benriya.Core.Abstractions.SystemUsers;
using Benriya.Core.Extensions;
using Benriya.Share.Abstractions;
using Benriya.Utils;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Modules.Core.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class ModulesController : CommonController
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<ModulesController> _logger;
        private readonly IStorage _storage;
        public ModulesController(ILogger<ModulesController> logger, IOptions<AppSettings> appSetting,IStorage storage,IRequestServices request) : base(request)
        {
            _appSettings = appSetting.Value;
            _logger = logger;
            _storage = storage;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var result = new ApiResultModel<List<IExtension>>();
            if (_appSettings.AssemblyCandidate)
            {
                string[] starts = new string[] { "ExtCore" };
                result.Data = ExtensionManager.GetInstances<IExtension>().Where(x => !starts.Any(sl => x.Name.StartsWith(sl))).ToList();
            }
            else            
                result.Data = ExtensionManager.GetInstances<IExtension>().ToList();
           
            if (result.Data != null)
                return Ok(result);
            else { 
                result.Notfound();
                return NotFound(result);
            }
        }

        [HttpGet("AppVersion")]
        public ActionResult AppVersion()
        {            
            return Ok(new ApiResultModel<Version>() { Data= Assembly.GetExecutingAssembly().GetName().Version });            
        }

        [HttpPost("{type}")]
        public async Task<ActionResult> Setup(string type)
        {
            if (type == null || !type.Equals("setup"))
                throw new ArgumentException("Type is required, or not match");
            var result = new ApiResultModel<bool>();
            var setup = await _storage.GetRepository<IPolicy_Repository>().SetupMudules();
            if (setup)
            {
                result.Data = setup;
                return Ok(result);
            }
            else
            {
                result.Notfound();
                return NotFound(result);
            }
        }
    }
}
