using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Share.Abstractions
{
    public interface ICommon_Repository<T,T1>
    {
        Task<T> SaveChangeAsync(T idata);        
        Task<T> CreateAsync(T idata);
        Task<T> UpdateAsync(T idata);
        Task<bool> RemoveAsync(T1 id);
        Task<bool> DeleteAsync(T1 id);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> func, int limit = 20);
        Task<T> GetAsync(Expression<Func<T, bool>> func);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        PagedList<T> GetList(PagingParams pagingParams, SearchOptions condition = null);
    }
}
