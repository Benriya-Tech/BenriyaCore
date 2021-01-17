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
    public class TagsGroup_Repository : RepositoryBase<Tags_Group>, ITagsGroup_Repository
    {
        private Tags_Group SetClient(Tags_Group idata, bool is_new = false)
        {
            if (is_new)
            {
                idata.id = 0;
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


        public async Task<Tags_Group> GetAsync(Expression<Func<Tags_Group, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }


        public async Task<List<Tags_Group>> ListAsync(Expression<Func<Tags_Group, bool>> func, int limit = 20)
        {
            var query = this.dbSet.Where(func);
            if (limit > 0)
                query = query.Take(limit);
            return await query.OrderBy(o => o.name).ToListAsync();
        }

        public async Task<Tags_Group> CreateAsync(Tags_Group idata)
        {
            if (idata.name.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Tags_Group> UpdateAsync(Tags_Group idata)
        {
            if (idata.id > 0)
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

        public async Task<bool> RemoveAsync(int id)
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

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new KeyNotFoundException($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Tags_Group> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        orderby data.id descending
                        select new Tags_Group()
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
            return new PagedList<Tags_Group>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            label = data.name,
                            id = data.id,
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
