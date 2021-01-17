using ExtCore.Data.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using Benriya.Modules.CMS.Share.ViewModels;

namespace Benriya.Modules.CMS.Entities.Abstractions
{
    public interface IContents_Repository : IRepository
    {
        Task<Contents> CreateAsync(Contents idata);
        Task<bool> DeleteAsync(Guid id);
        Task<Contents> GetAsync(Expression<Func<Contents, bool>> func);
        Task<List<Contents>> ListAsync(Expression<Func<Contents, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Contents> SaveChangeAsync(Contents idata);
        Task<Contents> UpdateAsync(Contents idata);
        PagedList<Content_ViewModel> GetList(PagingParams pagingParams, SearchOptions condition = null);
    }
}