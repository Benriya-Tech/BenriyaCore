using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Models;
using Benriya.Share.Abstractions;

namespace Benriya.Modules.UserManagement.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GuestController : CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<GuestController> _logger;
        public GuestController(IStorage storage, ILogger<GuestController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            RequestInfo client = new RequestInfo() { Client = _request.Info,CurrentUser = _request.CurrentUser};
            return Ok(client);
        }

        /// <summary>
        /// Register new users
        /// </summary>
        /// <returns>User object</returns>
        [HttpPost("Register")]
        public ActionResult Register(Users idata)
        {
            return Ok(idata);
        }
        [HttpPost("Login")]
        public ActionResult Login()
        {
            return Ok("Users test");
        }
    }
}
