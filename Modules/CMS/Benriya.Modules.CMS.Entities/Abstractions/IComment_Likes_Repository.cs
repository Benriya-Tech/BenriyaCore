using ExtCore.Data.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;

namespace Benriya.Modules.CMS.Entities.Abstractions
{
    public interface IComment_Likes_Repository : IRepository
    {
        Task<Comment_Likes> SaveChangeAsync(Comment_Likes idata);
        Task<Comment_Likes> CreateAsync(Comment_Likes idata);
        Task<Comment_Likes> UpdateAsync(Comment_Likes idata);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<Comment_Likes> GetAsync(Expression<Func<Comment_Likes, bool>> func);
        Task<List<Comment_Likes>> ListAsync(Expression<Func<Comment_Likes, bool>> func, int limit = 20);
    }
}