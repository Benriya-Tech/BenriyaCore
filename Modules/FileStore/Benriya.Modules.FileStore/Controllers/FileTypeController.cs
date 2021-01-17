using Benriya.Utils;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Benriya.Share.Abstractions;
using Benriya.Utils.Models;
using Benriya.Share.Models.FileStore;
using Benriya.Core.Abstractions.FileStore;

namespace Benriya.Modules.FileStore.Controllers
{
    [Route("api/FileStore/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FileTypeController:CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<FileTypeController> _logger;
        public FileTypeController(IStorage storage, ILogger<FileTypeController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve data by ID
        /// </summary>
        /// <param name="id">The ID of the desired FileStore_Type</param>
        /// <returns>A status object and Data</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(int id)
        {
            if(id < 1)
                return BadRequest(new ApiResultError(404));
            var result = new ApiResultModel<FileStore_FileType>();
            var data = await _storage.GetRepository<IFileStore_Type_Repository>().GetAsync(x=>x.id == id);

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
        /// Create new FileStore_Type
        /// </summary>
        /// <param name="idata">FileStore_Type object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPost]
        public async Task<ActionResult> Create(FileStore_FileType idata)
        {
            var result = new ApiResultModel<FileStore_FileType>();
            var data = await this._storage.GetRepository<IFileStore_Type_Repository>().CreateAsync(idata);
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
        /// Edit the FileStore_Type
        /// </summary>
        /// <param name="id">The ID of FileStore_Type</param>
        /// <param name="idata">FileStore_Type object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id,FileStore_FileType idata)
        {
            var result = new ApiResultModel<FileStore_FileType>();
            if (idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }
            var data = await this._storage.GetRepository<IFileStore_Type_Repository>().UpdateAsync(idata);
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
        /// Remove the FileStore_Type
        /// </summary>
        /// <param name="id">FileStore_Type id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(int id)
        {            
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IFileStore_Type_Repository>().RemoveAsync(id);
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
            var result = new ApiPagingtModel<List<FileStore_FileType>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            try
            {
                var data = _storage.GetRepository<IFileStore_Type_Repository>().GetList(new PagingParams());// new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
                Response.Headers.Add("X-Pagination", data.GetHeader().ToJson());
                result.Paging = data.GetHeader();
                result.Data = data.List;
                return Ok(result);
            }catch(Exception)
            {
                result.BadRequest();
                return BadRequest(result); 
            }
        }

        /// <summary>
        /// Completed delete the FileStore_Type
        /// </summary>
        /// <param name="id">FileStore_Type id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IFileStore_Type_Repository>().DeleteAsync(id);
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
        /// Remove the FileStore_Type
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Dropdown")]
        public async Task<ActionResult> Dropdown(string q = null,int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<DropdownItem>>();
            var data = await _storage.GetRepository<IFileStore_Type_Repository>().DropdownListAsync(q,limit);
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
        /// Remove the FileStore_Type
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Items")]
        public async Task<ActionResult> Items(string q = null, int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<FileStore_FileType>>();
            var data = await _storage.GetRepository<IFileStore_Type_Repository>().ItemsAynce(q, limit);
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

        [HttpPost("{type}")]
        public async Task<ActionResult> Setup(string type)
        {
            if (type == null || !type.Equals("setup"))
                throw new ArgumentException("Type is required, or not match");
            var result = new ApiResultModel<bool>();
            var setup = await _storage.GetRepository<IFileStore_Type_Repository>().SetupFIleType();
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
