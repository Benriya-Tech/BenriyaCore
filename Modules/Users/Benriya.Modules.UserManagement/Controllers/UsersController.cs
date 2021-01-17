using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Models.SystemUsers;
using Benriya.Core.Repositories.SystemUsers;
using System.Threading.Tasks;
using Benriya.Core.Abstractions.SystemUsers;
using Benriya.Utils.Pagingation;
using Benriya.Utils;
using System.Linq;
using System.Collections.Generic;
using Benriya.Utils.Models;
using Benriya.Core.Abstractions;
using Benriya.Share.Abstractions;
using Benriya.Utils.Extensions;
using System;
using Benriya.Share.ViewModels;
using MapsterMapper;

namespace Benriya.Modules.UserManagement.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class UsersController : CommonController
    {
        private readonly IStorage _storage;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        public UsersController(IStorage storage, ILogger<UsersController> logger,IRequestServices request,IMapper mapper) : base(request)
        {
            this._storage = storage;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user role
        /// </summary>
        /// <returns>Role object</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = new ApiResultModel<Users>();
            var data = await _storage.GetRepository<IUsers_Repository>().GetAsync(x => x.id == id);
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

        /// <summary>
        /// Get user role
        /// </summary>
        /// <returns>Role object</returns>
        [HttpGet]
        [Route("ToEdit/{id}")]
        public async Task<ActionResult> ToEdit(Guid id)
        {
            var result = new ApiResultModel<UsersEditModel>();
            var data = await _storage.GetRepository<IUsers_Repository>().GetAsync(x => x.id == id);
            if (data != null)
            {
                result.Data = _mapper.Map<UsersEditModel>(data);
                return Ok(result);
            }
            else
            {
                result.Notfound();
                return NotFound(result);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create(UsersEditModel idata)
        {
            if(idata.password.isNOEOW())
                throw new ArgumentException("Password is required");
            if (!idata.password.Equals(idata.confirm_password))
                throw new ArgumentException("Password missmatch");


            var result = new ApiResultModel<Users>();
            var data = await _storage.GetRepository<IUsers_Repository>().CreateAsync(_mapper.Map<Users>(idata),idata.password);
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
        public async Task<ActionResult> Update(Guid id, UsersEditModel idata)
        {
            if (!idata.password.isNOEOW() && (!idata.password.Equals(idata.confirm_password)))
                throw new ArgumentException("Password missmatch");

            var result = new ApiResultModel<UsersEditModel>();
            if(idata.id != id)
            {
                result.BadRequest("ID mismatch");
                return BadRequest(result);
            }
            var data = await _storage.GetRepository<IUsers_Repository>().UpdateAsync(_mapper.Map<Users>(idata));
            if (data != null)
            {
                result.Data = idata;
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
        public ActionResult List(int page = 0, int pageSize = 20)
        {
            var result = new ApiPagingtModel<List<Users>>();
            if (pageSize > 100)
            {                
                result.BadRequest("Limited 100 rows");
                return BadRequest(result);
            }
            var data = _storage.GetRepository<IUsers_Repository>().GetList(new PagingParams() { PageNumber = page, PageSize = pageSize }, new SearchOptions());
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
        public async Task<ActionResult> Remove(Guid id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IUsers_Repository>().RemoveAsync(id);
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
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = new ApiResultModel<bool>();
            var data = await this._storage.GetRepository<IUsers_Repository>().DeleteAsync(id);
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