using Benriya.Share.Models.CoreTags;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.AbstractTags
{
    public interface ITags_Repository : IRepository
    {
        Task<Tags> CreateAsync(Tags idata);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Tags> GetAsync(Expression<Func<Tags, bool>> func);
        PagedList<Tags> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Tags>> ListAsync(Expression<Func<Tags, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Tags> UpdateAsync(Tags idata);
    }
}