using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Pagingation;
using Benriya.Modules.CMS.Share.Models;
using Benriya.Utils.Models;

namespace Benriya.Modules.CMS.Entities.Abstractions
{
    public interface ICategory_Repository : IRepository
    {
        Task<Category> GetAsync(Expression<Func<Category, bool>> func);
        Task<List<Category>> ListAsync(Expression<Func<Category, bool>> func, int limit = 20);
        Task<List<Category>> ListAsync_Contents(Expression<Func<Category, bool>> func, int limit = 20);
        Task<Category> SaveChangeAsync(Category idata);
        Task<Category> CreateAsync(Category idata);
        Task<Category> UpdateAsync(Category idata);
        Task<bool> RemoveAsync(int id);
        Task<bool> DeleteAsync(int id);
        PagedList<Category> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<IEnumerable<Category>> ItemsAynce(string txt = null, int limit = 20);
        Task<IEnumerable<DropdownItem>> TagsDropdownAsync(int id, string txt = null, int limit = 20);
    }
}