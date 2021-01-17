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
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Modules.Inventory.Entities.Abstractions;

namespace Benriya.Modules.Inventory.Entities.Repositories
{
    public class Warehouse_Store_Repository : RepositoryBase<Warehouse_Store>, IWarehouse_Store_Repository
    {
        private Warehouse_Store SetClient(Warehouse_Store idata, bool is_new = false)
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

        public async Task<Warehouse_Store> GetAsync(Expression<Func<Warehouse_Store, bool>> func)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(func);

            return data;
        }

        public async Task<List<Warehouse_Store>> ListAsync(Expression<Func<Warehouse_Store, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.created);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<Warehouse_Store> SaveChangeAsync(Warehouse_Store idata)
        {
            if (idata.id != Guid.Empty)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }
        public async Task<Warehouse_Store> CreateAsync(Warehouse_Store idata)
        {
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Warehouse_Store> UpdateAsync(Warehouse_Store idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
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

        public PagedList<Warehouse_Store> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Warehouse_Store()
                        {
                            id = data.id,
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
                    EF.Functions.Like(x.description, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Warehouse_Store>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.created descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
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
