using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.FileStore
{
    public interface IFileStore_Common<T>
    {
        Task<T> CreateAsync(T idata);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<T> GetAsync(Expression<Func<T, bool>> func);
        PagedList<T> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<T>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<T> SaveChangeAsync(T idata);
        Task<T> UpdateAsync(T idata);
    }
}