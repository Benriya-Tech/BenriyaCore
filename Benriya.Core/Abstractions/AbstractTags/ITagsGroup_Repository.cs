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
    public interface ITagsGroup_Repository : IRepository
    {
        Task<Tags_Group> CreateAsync(Tags_Group idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Tags_Group> GetAsync(Expression<Func<Tags_Group, bool>> func);
        PagedList<Tags_Group> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Tags_Group>> ListAsync(Expression<Func<Tags_Group, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<Tags_Group> UpdateAsync(Tags_Group idata);
    }
}