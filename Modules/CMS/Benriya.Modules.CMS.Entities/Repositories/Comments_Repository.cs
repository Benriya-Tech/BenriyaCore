using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Utils.Extensions;
using Benriya.Modules.CMS.Entities.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using Benriya.Modules.CMS.Share.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;

namespace Benriya.Modules.CMS.Entities.Repositories
{
    public class Comments_Repository : RepositoryBase<Comments>, IComments_Repository
    {
        private Comments SetClient(Comments idata, bool is_new = false)
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
        public async Task<Comments> GetAsync(Expression<Func<Comments, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }

        public async Task<List<Comments>> ListAsync(Expression<Func<Comments, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.created);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }
        public async Task<Comments> SaveChangeAsync(Comments idata)
        {
            if (idata.id != Guid.Empty)
            {
                return await UpdateAsync(idata);
            }
            else
            {
                return await CreateAsync(idata);
            }

        }

        public async Task<Comments> CreateAsync(Comments idata)
        {
            idata = SetClient(idata, true);
            idata.Contents = null;
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;

        }

        public async Task<Comments> UpdateAsync(Comments idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = SetClient(idata);
                data.description = idata.description;
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

        public PagedList<Comment_ViewModel> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var content_db = this.storageContext.Set<Contents>();
            //var user_db = this.storageContext.Set<Users>();
            var query = from data in this.dbSet
                        join content in content_db on data.content_id equals content.id
                        //join likes in likes_db on data.content_id equals likes.content_id
                        where data.is_active == true && content.is_active == true
                        select new Comment_ViewModel
                        {
                            id = data.id,
                            description = data.description,
                            content_id = content.id,
                            content_name = content.name,
                            likes = data.Comment_Likes.Count(),
                            created = data.created,
                            updated = data.updated,
                            created_by = data.created_by,
                            updated_by = data.updated_by
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.name, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.description, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Comment_ViewModel>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }



    }

}
