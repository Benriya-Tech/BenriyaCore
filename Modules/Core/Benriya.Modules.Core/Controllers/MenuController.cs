using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Abstractions;
using Benriya.Core.Abstractions.Menus;
using System.Threading.Tasks;
using Benriya.Utils;
using Benriya.Share.Models.Menus;
using System.Collections.Generic;
using Benriya.Utils.Pagingation;
using Microsoft.AspNetCore.Authorization;
using Benriya.Core.Filters;
using Benriya.Core.Extensions;
using Benriya.Share.ViewModels;

namespace Benriya.Modules.Core.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<MenuController> _logger;
        public MenuController(ILogger<MenuController> logger,IStorage storage,IRequestServices request) : base(request)
        {
            _storage = storage;
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
        public async Task<ActionResult> Index(int id)
        {
            var result = new ApiResultModel<SystemMenu>();
            var data = await _storage.GetRepository<IMenu_Repository>().GetAsync(x => x.id == id);
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
        public async Task<ActionResult> Create(SystemMenu idata)
        {
            var result = new ApiResultModel<SystemMenu>();
            var data = await _storage.GetRepository<IMenu_Repository>().CreateAsync(idata);
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
        public async Task<ActionResult> Update(int id, SystemMenu idata)
        {
            var result = new ApiResultModel<SystemMenu>();
            if (idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }

            var data = await _storage.GetRepository<IMenu_Repository>().UpdateAsync(idata);
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

        [HttpGet("List")]
        [ApiAuthurize]
        [ClientAuthorize(Extension.Policy,ClaimPermission.List)]
        public ActionResult List(int page = 0, int pageSize = 20,[FromQuery]SystemMenu_Filter filter = null)
        {
            var result = new ApiPagingtModel<List<SystemMenu>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            var data = _storage.GetRepository<IMenu_Repository>().GetList(new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
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
            var data = await this._storage.GetRepository<IMenu_Repository>().RemoveAsync(id);
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
            var data = await this._storage.GetRepository<IMenu_Repository>().DeleteAsync(id);
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


        /// <summary>
        /// List all of menu
        /// </summary>
        /// <returns>A status object</returns>
        [HttpGet("ListAsync")]
        public async Task<ActionResult> ListAsync()
        {
            var result = new ApiResultModel<List<SystemMenu>>();
            var data = await this._storage.GetRepository<IMenu_Repository>().ListAsync(x => x.is_active == true, 0);
            if (data != null)
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
