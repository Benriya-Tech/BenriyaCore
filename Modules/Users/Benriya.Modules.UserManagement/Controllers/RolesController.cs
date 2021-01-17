using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Models.SystemUsers;
using System.Threading.Tasks;
using Benriya.Core.Abstractions.SystemUsers;
using Benriya.Utils.Pagingation;
using Benriya.Utils;
using System.Collections.Generic;
using Benriya.Share.Abstractions;

namespace Benriya.Modules.UserManagement.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolesController : CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IStorage storage, ILogger<RolesController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Get user role
        /// </summary>
        /// <returns>Role object</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(int id)
        {
            var result = new ApiResultModel<User_Role>();
            var data = await _storage.GetRepository<IUserRole_Repository>().GetAsync(x => x.id == id);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.Notfound();
                return NotFound(result);
            }
        }

        


        [HttpPost]
        public async Task<ActionResult> Create(User_Role idata)
        {
            var result = new ApiResultModel<User_Role>();
            var data = await _storage.GetRepository<IUserRole_Repository>().CreateAsync(idata);
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

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(int id, User_Role idata)
        {
            var result = new ApiResultModel<User_Role>();
            if(idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }
            
            var data = await _storage.GetRepository<IUserRole_Repository>().UpdateAsync(idata);
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

        [HttpPost("Login")]
        public ActionResult Login()
        {
            return Ok("Users test");
        }

        [HttpGet]
        [Route("List")]
        public ActionResult List(int page = 0, int pageSize = 20)
        {
            var result = new ApiPagingtModel<List<User_Role>>();
            if (pageSize > 100)
            {                
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            var data = _storage.GetRepository<IUserRole_Repository>().GetList(new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
            Response.Headers.Add("X-Pagination", data.GetHeader().ToJson());
            result.Paging = data.GetHeader();
            result.Data = data.List;
            return Ok(result);
        }


        /// <summary>
        /// Remove the category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IUserRole_Repository>().RemoveAsync(id);
            if (data)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("Cannot remove data");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Completed delete the category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IUserRole_Repository>().DeleteAsync(id);
            if (data)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("Cannot delete data");
                return BadRequest(result);
            }
        }
    }
}