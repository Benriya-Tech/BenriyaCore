using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Abstractions;
using System.Threading.Tasks;
using Benriya.Utils;
using System.Collections.Generic;
using Benriya.Utils.Pagingation;
using Microsoft.AspNetCore.Authorization;
using Benriya.Core.Filters;
using Benriya.Core.Extensions;
using Benriya.Share.ViewModels;
using Benriya.Share.Models.SystemUsers;
using Benriya.Core.Abstractions.AbstractTags;
using Benriya.Share.Models.CoreTags;
using Benriya.Utils.Models;

namespace Benriya.Modules.Core.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController] 
    [Authorize]
    public class TagGroupController : CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<TagGroupController> _logger;
        public TagGroupController(ILogger<TagGroupController> logger,IStorage storage,IRequestServices request) : base(request)
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
            var result = new ApiResultModel<Tags_Group>();
            var data = await _storage.GetRepository<ITagsGroup_Repository>().GetAsync(x => x.id == id);
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
        public async Task<ActionResult> Create(Tags_Group idata)
        {
            var result = new ApiResultModel<Tags_Group>();
            var data = await _storage.GetRepository<ITagsGroup_Repository>().CreateAsync(idata);
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
        public async Task<ActionResult> Update(int id, Tags_Group idata)
        {
            var result = new ApiResultModel<Tags_Group>();
            if (idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }

            var data = await _storage.GetRepository<ITagsGroup_Repository>().UpdateAsync(idata);
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
        public ActionResult List(int page = 0, int pageSize = 20)
        {
            var result = new ApiPagingtModel<List<Tags_Group>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            var data = _storage.GetRepository<ITagsGroup_Repository>().GetList(new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
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
            var data = await this._storage.GetRepository<ITagsGroup_Repository>().RemoveAsync(id);
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
            var data = await this._storage.GetRepository<ITagsGroup_Repository>().DeleteAsync(id);
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
            var result = new ApiResultModel<List<Tags_Group>>();
            var data = await this._storage.GetRepository<ITagsGroup_Repository>().ListAsync(x => x.name != null, 0);
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

        /// <summary>
        /// List of tag category
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Dropdown")]
        public async Task<ActionResult> Dropdown(string q = null, int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<DropdownItem>>();
            var data = await _storage.GetRepository<ITagsGroup_Repository>().DropdownListAsync(q, limit);
            if (data != null)
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
    }
}
