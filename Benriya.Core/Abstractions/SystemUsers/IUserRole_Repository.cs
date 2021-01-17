using Benriya.Share.Models.SystemUsers;
using Benriya.Share.ViewModels;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.SystemUsers
{
    public interface IUserRole_Repository : IRepository
    {
        Task<User_Role> CreateAsync(User_Role idata);
        Task<bool> DeleteAsync(int id);
        Task<User_Role> GetAsync(Expression<Func<User_Role, bool>> func);
        Task<List<User_Role>> ListAsync(Expression<Func<User_Role, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<User_Role> UpdateAsync(User_Role idata);
        PagedList<User_Role> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20);
    }
}