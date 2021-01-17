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
    public interface IPermission_Repository : IRepository
    {
        Task<Permission_Access> CreateAsync(Permission_Access idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20);
        Task<Permission_Access> GetAsync(Expression<Func<Permission_Access, bool>> func);
        PagedList<Permission_Access> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Permission_Access>> ListAsync(Expression<Func<Permission_Access, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<Permission_Access> UpdateAsync(Permission_Access idata);
    }
}