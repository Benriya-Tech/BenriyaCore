﻿using Benriya.Utils;
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
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Modules.Inventory.Entities.Abstractions;
using Benriya.Core.Filters;
using Benriya.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Benriya.Share.ViewModels;

namespace Benriya.Modules.Inventory.Controllers
{
    [Route("api/inventory/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiAuthurize]
    [Authorize]
    public class WarehouseController:CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<WarehouseController> _logger;
        public WarehouseController(IStorage storage, ILogger<WarehouseController> logger,IRequestServices request) : base(request)
        {
            this._storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve data by ID
        /// </summary>
        /// <param name="id">The ID of the desired Warehouse</param>
        /// <returns>A status object and Data</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(int id)
        {
            if(id < 1)
                return BadRequest(new ApiResultError(404));
            var result = new ApiResultModel<Warehouse>();
            var data = await _storage.GetRepository<IWarehouse_Repository>().GetAsync(x=>x.id == id);

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
        /// Create new Warehouse
        /// </summary>
        /// <param name="idata">Warehouse object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPost]
        public async Task<ActionResult> Create(Warehouse idata)
        {
            var result = new ApiResultModel<Warehouse>();
            var data = await this._storage.GetRepository<IWarehouse_Repository>().CreateAsync(idata);
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
        /// Edit the Warehouse
        /// </summary>
        /// <param name="id">The ID of Warehouse</param>
        /// <param name="idata">Warehouse object class</param>
        /// <returns>A status object and Data</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id,Warehouse idata)
        {
            var result = new ApiResultModel<Warehouse>();
            if (idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }
            var data = await this._storage.GetRepository<IWarehouse_Repository>().UpdateAsync(idata);
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
        /// Remove the Warehouse
        /// </summary>
        /// <param name="id">Warehouse id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(int id)
        {            
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IWarehouse_Repository>().RemoveAsync(id);
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
        [ClientAuthorize(Extension.Policy,ClaimPermission.List)] // Inventory.List
        public ActionResult List(int page = 0, int pageSize = 20, string txt = null)
        {
            var result = new ApiPagingtModel<List<Warehouse>>();
            if (pageSize > 100)
            {
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            try
            {
                var data = _storage.GetRepository<IWarehouse_Repository>().GetList(new PagingParams());// new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
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
        /// Completed delete the Warehouse
        /// </summary>
        /// <param name="id">Warehouse id</param>
        /// <returns>A status object</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IWarehouse_Repository>().DeleteAsync(id);
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
        /// Remove the Warehouse
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Dropdown")]
        public async Task<ActionResult> Dropdown(string q = null,int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<DropdownItem>>();
            var data = await _storage.GetRepository<IWarehouse_Repository>().DropdownListAsync(q,limit);
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
        /// Remove the Warehouse
        /// </summary>
        /// <param name="q">Text search</param>
        /// <param name="limit">List of data per request</param>
        /// <returns>A status object</returns>
        [HttpGet]
        [Route("Items")]
        public async Task<ActionResult> Items(string q = null, int limit = 20)
        {
            var result = new ApiResultModel<IEnumerable<Warehouse>>();
            var data = await _storage.GetRepository<IWarehouse_Repository>().ItemsAynce(q, limit);
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
        /// Check area data
        /// </summary>
        /// <param name="idata">Warehouse area data</param>
        /// <returns>A status object</returns>
        [HttpPost]
        [Route("CheckArea")]
        public async Task<ActionResult> CheckArea(Warehouse_Area idata)
        {
            var result = new ApiResultModel<Warehouse_Area>();
            var data = await this._storage.GetRepository<IWarehouse_Repository>().CheckArea(idata);
            if (data != null)
            {
                result.Data = data;
                return Ok(result);
            }
            else
            {
                result.BadRequest("Please check your data");
                return BadRequest(result);
            }
        }

    }
}
