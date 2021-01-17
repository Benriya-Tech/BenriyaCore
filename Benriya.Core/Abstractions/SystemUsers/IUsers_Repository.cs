using Benriya.Core.Abstractions;
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
    public interface IUsers_Repository : IRepository
    {
        Task<Users> CreateAsync(Users idata, string password);
        Task<bool> DeleteAsync(Guid id);
        Task<Users> GetAsync(Expression<Func<Users, bool>> func);
        PagedList<Users> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Users>> ListAsync(Expression<Func<Users, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Users> UpdateAsync(Users idata, string password = null);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20);
        Task<Users> Authen_Validate(string username, string password);
    }
}