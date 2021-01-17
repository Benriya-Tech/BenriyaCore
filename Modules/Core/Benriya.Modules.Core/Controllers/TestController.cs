using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Benriya.Modules.Core.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Produces("application/json")]
    
    public class TestController : CommonController
    {
        //private readonly IStorage _storage;
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger,IRequestServices request) : base(request) //IStorage storage
        {
            //this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve Index
        /// </summary>
        /// <param name="id">The ID of the desired Employee</param>
        /// <returns>A string status</returns>
        [HttpGet]
        [Route("{id}")]
        //[ApiAutherize]
        public ActionResult Index(int id)
        {
            _logger.LogDebug("test xxx");
            //var persons = this.storage.GetRepository<IPersonRepository>().First();
            return Ok("Test Page : "+id);// + persons.name);//this.View(ExtensionManager.GetInstances<IExtension>().Select(e => e.Name));

        }

        [HttpGet]
        [Route("check")]        
        //[CustomAuthorize("CMS","edit")]
        public ActionResult Auth()
        {

            //var persons = this.storage.GetRepository<IPersonRepository>().First();
            return Ok("Check Page : ");// + persons.name);//this.View(ExtensionManager.GetInstances<IExtension>().Select(e => e.Name));

        }
        /// <summary>
        /// Returns a group of Employees matching the given first and last names.
        /// </summary>
        /// <remarks>
        /// Here is a sample remarks placeholder.
        /// </remarks>
        /// <param name="firstName">The first name to search for</param>
        /// <param name="lastName">The last name to search for</param>
        /// <returns>A string status</returns>
        [HttpGet]
        [Route("byname/{firstName}/{lastName}")]
        public ActionResult<string> GetByName(string firstName, string lastName)
        {
            return "Found another employee";
        }
    }
}
