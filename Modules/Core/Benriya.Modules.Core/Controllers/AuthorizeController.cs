using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Abstractions;
using Benriya.Share.ViewModels;
using Benriya.Core.Services;
using System.Threading.Tasks;
using Benriya.Utils;
using Benriya.Core.Filters;

namespace Benriya.Modules.Core.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiAuthurize]
    public class AuthorizeController : CommonController
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly IUserServices _userService;
        public AuthorizeController(ILogger<AuthorizeController> logger,IRequestServices request,IUserServices userService) : base(request)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Retrieve Index
        /// </summary>
        /// <param name="id">The ID of the desired Employee</param>
        /// <returns>A string status</returns>
        [HttpGet("{id}")]
        //[ApiAutherize]
        public ActionResult Index(int id)
        {
            _logger.LogDebug("test xxx");
            //var persons = this.storage.GetRepository<IPersonRepository>().First();
            return Ok("Test Page : "+id);// + persons.name);//this.View(ExtensionManager.GetInstances<IExtension>().Select(e => e.Name));

        }

        [HttpPost]
        public async Task<ActionResult> Authen(UserLoginModel idata)     
        {
            
            var result = new ApiResultModel<UserInfoModel>();
            var data = await _userService.Authenticate(idata.email,idata.password);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest();
                return BadRequest(result);
            }
        }
    }
}
