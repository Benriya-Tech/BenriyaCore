using Benriya.Utils;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Modules.CMS.Entities.Abstractions;
using System;
using System.Threading.Tasks;
using Benriya.Modules.CMS.Share.Models;
using System.Collections.Generic;
using Benriya.Modules.CMS.Share.ViewModels;
using Benriya.Share.Abstractions;

namespace Benriya.Modules.CMS.Controllers
{
    [Route("api/cms/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommentController:CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<CommentController> _logger;
        public CommentController(IStorage storage, ILogger<CommentController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve data by ID
        /// </summary>
        /// <param name="id">The ID of the desired Comment</param>
        /// <returns>A status object and Data</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest(new ApiResultError(404));
            var result = new ApiResultModel<Comments>();
            var data = await this._storage.GetRepository<IComments_Repository>().GetAsync(x=>x.id == id);
            if (data != null) {
                result.Data = data;
                return Ok(result);
            }
            else {
                result.BadRequest("Data not found");
                return NotFound(result);
            }            
        }

        /// <summary>
        /// Create new content
        /// </summary>
        /// <param name="idata">Content object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPost]
        public async Task<ActionResult> Create(Comments idata)
        {
            var result = new ApiResultModel<Comments>();
            var data = await this._storage.GetRepository<IComments_Repository>().CreateAsync(idata);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else {
                result.BadRequest("Cannot save data");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Edit the content
        /// </summary>
        /// <param name="id">Content ID</param>
        /// <param name="idata">Content object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Comments idata)
        {
            var result = new ApiResultModel<Comments>();
            var data = await this._storage.GetRepository<IComments_Repository>().UpdateAsync(idata);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("Cannot edit data");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Remove the Content
        /// </summary>
        /// <param name="id">Content id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IComments_Repository>().RemoveAsync(id);
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
        /// Retrieve list of data and pagination
        /// </summary>
        /// <param name="page">Get data of page number</param>
        /// <param name="pageSize">List of data per request</param>
        /// <param name="txt">Search data by text</param>
        /// <returns>A status object and Data</returns>
        [HttpGet("List")]
        public ActionResult List(int page = 1, int pageSize = 20, string txt = null)
        {
            var result = new ApiPagingtModel<List<Comment_ViewModel>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            var data = this._storage.GetRepository<IComments_Repository>().GetList(new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions() { txt = txt });
            Response.Headers.Add("X-Pagination", data.GetHeader().ToJson());
            result.Paging = data.GetHeader();
            result.Data = data.List;
            return Ok(result);
        }

        /// <summary>
        /// Completed delete the content
        /// </summary>
        /// <param name="id">Content id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IComments_Repository>().DeleteAsync(id);
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
