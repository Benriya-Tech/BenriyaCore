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
    public class SysController : CommonController
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<SysController> _logger;
        private readonly IStorage _storage;
        public SysController(ILogger<SysController> logger, IOptions<AppSettings> appSetting,IStorage storage,IRequestServices request) : base(request)
        {
            _appSettings = appSetting.Value;
            _logger = logger;
            _storage = storage;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return NotFound(new ApiResultError(404));
        }

        [HttpGet]
        [Route("Version")]
        public ActionResult MainVersion()
        {            
            return Ok(new ApiResultModel<string>() { Data= Assembly.GetExecutingAssembly().GetName().Version.ToString() });            
        }
        
    }
}
