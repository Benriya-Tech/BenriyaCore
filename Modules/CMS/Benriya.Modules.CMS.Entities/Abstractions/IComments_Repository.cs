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
    public interface IComments_Repository : IRepository
    {
        Task<Comments> CreateAsync(Comments idata);
        Task<bool> DeleteAsync(Guid id);
        Task<Comments> GetAsync(Expression<Func<Comments, bool>> func);
        PagedList<Comment_ViewModel> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Comments>> ListAsync(Expression<Func<Comments, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Comments> SaveChangeAsync(Comments idata);
        Task<Comments> UpdateAsync(Comments idata);
    }
}