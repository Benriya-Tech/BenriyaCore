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
using Benriya.Share.Abstractions;
using Benriya.Utils.Models;

namespace Benriya.Modules.CMS.Controllers
{
    [Route("api/cms/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController:CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(IStorage storage, ILogger<CategoryController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve data by ID
        /// </summary>
        /// <param name="id">The ID of the desired Category</param>
        /// <returns>A status object and Data</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(int id)
        {
            if(id < 1)
                return BadRequest(new ApiResultError(404));
            var result = new ApiResultModel<Category>();
            var data = await _storage.GetRepository<ICategory_Repository>().GetAsync(x=>x.id == id);

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
        /// Create new category
        /// </summary>
        /// <param name="idata">Category object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPost]
        public async Task<ActionResult> Create(Category idata)
        {
            var result = new ApiResultModel<Category>();
            var data = await this._storage.GetRepository<ICategory_Repository>().CreateAsync(idata);
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
        /// Edit the category
        /// </summary>
        /// <param name="id">The ID of Category</param>
        /// <param name="idata">Category object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id,Category idata)
        {
            var result = new ApiResultModel<Category>();
            if (idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }
            var data = await this._storage.GetRepository<ICategory_Repository>().UpdateAsync(idata);
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
        /// Remove the category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(int id)
        {            
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<ICategory_Repository>().RemoveAsync(id);
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
        public ActionResult List(int page = 0, int pageSize = 20, string txt = null)
        {
            var result = new ApiPagingtModel<List<Category>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
    
            var data = _storage.GetRepository<ICategory_Repository>().GetList(new PagingParams());// new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
            Response.Headers.Add("X-Pagination", data.GetHeader().ToJson());
            result.Paging = data.GetHeader();
            result.Data = data.List;
            return Ok(result);

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
            var data = await this._storage.GetRepository<ICategory_Repository>().DeleteAsync(id);
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
        /// Remove the category
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Dropdown")]
        public async Task<ActionResult> Dropdown(string q = null,int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<DropdownItem>>();
            var data = await _storage.GetRepository<ICategory_Repository>().DropdownListAsync(q,limit);
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

        /// <summary>
        /// Remove the category
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Items")]
        public async Task<ActionResult> Items(string q = null, int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<Category>>();
            var data = await _storage.GetRepository<ICategory_Repository>().ItemsAynce(q, limit);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("No data found");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Remove the category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Tags")]
        public async Task<ActionResult> Tags(int id,string q = null, int limit = 20)
        {
            if (id < 1)
                throw new ArgumentNullException("CMS Category ID is required");

            var result = new ApiResultModel<IEnumerable<DropdownItem>>();
            var data = await _storage.GetRepository<ICategory_Repository>().TagsDropdownAsync(id,q, limit);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("No data found");
                return BadRequest(result);
            }
        }

    }
}
