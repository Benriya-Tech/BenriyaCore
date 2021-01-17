using Benriya.Modules.eCommerce.Entities.Abstractions;
using Benriya.Modules.eCommerce.Share.Models.Orders;
using Benriya.Utils.Extensions;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Modules.eCommerce.Entities.Repositories.Orders
{
    class Order_Detail_Repository : RepositoryBase<Order_Detail>, IOrder_Detail_Repository
    {
        private Order_Detail SetClient(Order_Detail idata, bool is_new = false)
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
        //private async Task<Order_Detail> CheckData(Order_Detail idata)
        //{
        //    if (idata == null)
        //        throw new NullReferenceException("Order_Detail data is required");
        //    //var name = await this.dbSet.Select(x => new { id = x.id, name = x.name }).FirstOrDefaultAsync(x => x.id != idata.id && x.name == idata.name);
        //    //if (name != null)
        //    //    throw new DuplicateNameException("[name]", new Exception($"Category name [{idata.name}] is already used by existing object"));

        //    return idata;
        //}

        public async Task<Order_Detail> CreateAsync(Order_Detail idata)
        {
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Order_Detail> GetAsync(Expression<Func<Order_Detail, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }
        public async Task<List<Order_Detail>> ListAsync(Expression<Func<Order_Detail, bool>> func, int limit = 20)
        {
            if (limit > 0)
                return await this.dbSet.Where(func).Take(limit).ToListAsync();
            return await this.dbSet.Where(func).ToListAsync();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var data = await GetAsync(x => x.id == id);
            if (data != null)
            {
                this.SetClient(data);
                data.is_active = false;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order_Detail> SaveChangeAsync(Order_Detail idata)
        {
            if (idata.id != Guid.Empty)
                return await this.UpdateAsync(idata);
            else
                return await this.CreateAsync(idata);
        }

        public async Task<Order_Detail> UpdateAsync(Order_Detail idata)
        {
            var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
            if (data == null)
                throw new Exception($"Order_Detail id:{idata.id} is not found");
            idata = SetClient(idata);
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return data;
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.created descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            uid = data.id,
                            label = data.id.ToString(),
                            description = data.created.ToString()
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id.Equals(id));
            if (data == null)
                throw new Exception($"Category id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Order_Detail> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Order_Detail()
                        {
                            id = data.id,
                            created = data.created,
                            updated = data.updated,
                            price = data.price,
                            goods_id = data.goods_id,
                            is_active = data.is_active,
                            original_price = data.original_price,
                            promo_id = data.promo_id
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Order_Detail>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }
    }
}
