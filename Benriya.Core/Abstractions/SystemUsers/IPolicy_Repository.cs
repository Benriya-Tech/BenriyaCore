using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.SystemUsers
{
    public interface IPolicy_Repository : IRepository
    {
        Task<Policy_Roles> CreateAsync(Policy_Roles idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20);
        Task<Policy_Roles> GetAsync(Expression<Func<Policy_Roles, bool>> func);
        PagedList<Policy_Roles> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Policy_Roles>> ListAsync(Expression<Func<Policy_Roles, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<bool> SetupMudules();
        Task<Policy_Roles> UpdateAsync(Policy_Roles idata);
    }
}