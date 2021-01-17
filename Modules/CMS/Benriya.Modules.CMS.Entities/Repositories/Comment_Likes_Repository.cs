using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Modules.CMS.Entities.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;

namespace Benriya.Modules.CMS.Entities.Repositories
{
    public class Comment_Likes_Repository : RepositoryBase<Comment_Likes>, IComment_Likes_Repository
    {
        private Comment_Likes SetClient(Comment_Likes idata, bool is_new = false)
        {
            if (is_new)
            {
                idata.created_by = Client.CurrentUser.id;
                idata.created_ip = Client.Info.ipAddress;
            }
            else
            {
                idata.updated = DateTime.Now;
                idata.updated_by = Client.CurrentUser.id;
                idata.updated_ip = Client.Info.ipAddress;
            }
            return idata;
        }
        public async Task<Comment_Likes> GetAsync(Expression<Func<Comment_Likes, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }

        public async Task<List<Comment_Likes>> ListAsync(Expression<Func<Comment_Likes, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.created);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<Comment_Likes> SaveChangeAsync(Comment_Likes idata)
        {
            if (idata.id != Guid.Empty)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }
        public async Task<Comment_Likes> CreateAsync(Comment_Likes idata)
        {
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Comment_Likes> UpdateAsync(Comment_Likes idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = SetClient(idata);
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            data = SetClient(data);
            data.is_active = false;
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

    }

}
