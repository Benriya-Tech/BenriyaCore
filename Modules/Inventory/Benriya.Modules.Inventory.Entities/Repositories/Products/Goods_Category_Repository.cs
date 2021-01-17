using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Pagingation;
using Benriya.Utils.Models;
using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Modules.Inventory.Entities.Abstractions;
using System.Data;

namespace Benriya.Modules.Inventory.Entities.Repositories
{
    public class Goods_Category_Repository : RepositoryBase<Goods_Category>, IGoods_Category_Repository
    {
        private Goods_Category SetClient(Goods_Category idata, bool is_new = false)
        {
            if (is_new)
            {
                idata.created_by = Client.CurrentUser.id;
                idata.created = DateTime.Now;
            }
            else
            {
                idata.updated = DateTime.Now;
                idata.updated_by = Client.CurrentUser.id;
            }
            return idata;
        }
        private async Task<Goods_Category> CheckData(Goods_Category idata)
        {
            if (idata == null)
                throw new NullReferenceException("Category data is required");
            var name = await this.dbSet.Select(x => new { id = x.id, name = x.name }).FirstOrDefaultAsync(x => x.id != idata.id && x.name == idata.name);
            if (name != null)
                throw new DuplicateNameException("[name]", new Exception($"Category name is already used by existing object"));

            return idata;
        }

        public async Task<Goods_Category> GetAsync(Expression<Func<Goods_Category, bool>> func)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(func);
            return data;
        }

        public async Task<List<Goods_Category>> ListAsync(Expression<Func<Goods_Category, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<Goods_Category> SaveChangeAsync(Goods_Category idata)
        {
            if (idata.id > 0)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }
        public async Task<Goods_Category> CreateAsync(Goods_Category idata)
        {
            await CheckData(idata);
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Goods_Category> UpdateAsync(Goods_Category idata)
        {
            if (idata.id > 0)
            {
                await CheckData(idata);
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Category id:{idata.id} is not found");
                idata = SetClient(idata);
                data.name = idata.name;
                data.description = idata.description;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Category id:{id} is not found");
            data = SetClient(data);
            data.is_active = false;
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Category id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Goods_Category> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Goods_Category()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated
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
            return new PagedList<Goods_Category>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            id = data.id,
                            label = data.name,
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

        public async Task<IEnumerable<Goods_Category>> ItemsAynce(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new Goods_Category()
                        {
                            id = data.id,
                            name = data.name
                        };
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{txt}%") ||
                    EF.Functions.Like(x.name, $"%{txt}%")
                //EF.Functions.Like(x.description, $"%{txt}%")
                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }
    }

}
