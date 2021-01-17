using Benriya.Share.Models.CoreTags;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Benriya.Utils.Extensions;
using Benriya.Utils.Pagingation;
using System.Threading.Tasks;
using Benriya.Utils.Models;
using Benriya.Core.Abstractions.AbstractTags;

namespace Benriya.Core.Repositories.TagsRepo
{
    public class Tags_Repository : RepositoryBase<Tags>, ITags_Repository
    {
        private Tags SetClient(Tags idata, bool is_new = false)
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
            if (idata.Group != null)
                idata.group_id = idata.Group.id;
            if (idata.InGroup != null && idata.InGroup.id > 0)
                idata.group_id = idata.InGroup.id;

            idata.Group = null;
            return idata;
        }


        public async Task<Tags> GetAsync(Expression<Func<Tags, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }


        public async Task<List<Tags>> ListAsync(Expression<Func<Tags, bool>> func, int limit = 20)
        {
            var query = this.dbSet.Where(func);
            if (limit > 0)
                query = query.Take(limit);
            return await query.OrderBy(o => o.name).ToListAsync();
        }

        public async Task<Tags> CreateAsync(Tags idata)
        {
            if (idata.name.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Tags> UpdateAsync(Tags idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Permission id:{idata.id} is not found");
                data.name = idata.name;
                data = SetClient(data);
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
                if (data == null)
                    throw new Exception($"Permission id:{id} is not found");
                data = SetClient(data);
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new KeyNotFoundException($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Tags> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        orderby data.id descending
                        select new Tags()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
                            created_by = data.created_by
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
            return new PagedList<Tags>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            label = data.name,
                            uid = data.id,
                            description = data.description
                        };
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.value, $"%{txt}%") ||
                    EF.Functions.Like(x.label, $"%{txt}%") ||
                    EF.Functions.Like(x.description, $"%{txt}%")
                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }

    }
}
