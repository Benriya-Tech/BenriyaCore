using Benriya.Share.Models.Menus;
using Benriya.Share.ViewModels;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.Menus
{
    public interface IMenu_Repository : IRepository
    {
        Task<SystemMenu> CreateAsync(SystemMenu idata);
        Task<bool> DeleteAsync(int id);
        Task<SystemMenu> GetAsync(Expression<Func<SystemMenu, bool>> func);
        PagedList<SystemMenu> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<SystemMenu>> ListAsync(Expression<Func<SystemMenu, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<SystemMenu> UpdateAsync(SystemMenu idata);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20);
    }
}