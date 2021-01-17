using ExtCore.Data.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;

namespace Benriya.Modules.CMS.Entities.Abstractions
{
    public interface IContent_Likes_Repository : IRepository
    {
        Task<Content_Likes> SaveChangeAsync(Content_Likes idata);
        Task<Content_Likes> CreateAsync(Content_Likes idata);
        Task<Content_Likes> UpdateAsync(Content_Likes idata);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<Content_Likes> GetAsync(Expression<Func<Content_Likes, bool>> func);
        Task<List<Content_Likes>> ListAsync(Expression<Func<Content_Likes, bool>> func, int limit = 20);
    }
}