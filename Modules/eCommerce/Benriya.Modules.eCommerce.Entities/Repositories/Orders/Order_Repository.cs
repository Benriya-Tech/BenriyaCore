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
    public class Order_Repository : RepositoryBase<Order>, IOrder_Repository
    {
        private Order SetClient(Order idata, bool is_new = false)
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
        //private async Task<Order> CheckData(Order idata)
        //{
        //    if (idata == null)
        //        throw new NullReferenceException("Order data is required");
        //    //var name = await this.dbSet.Select(x => new { id = x.id, name = x.name }).FirstOrDefaultAsync(x => x.id != idata.id && x.name == idata.name);
        //    //if (name != null)
        //    //    throw new DuplicateNameException("[name]", new Exception($"Category name [{idata.name}] is already used by existing object"));

        //    return idata;
        //}

        public async Task<Order> CreateAsync(Order idata)
        {
            idata = SetClient(idata,true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Order> GetAsync(Expression<Func<Order, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }
        public async Task<List<Order>> ListAsync(Expression<Func<Order, bool>> func, int limit = 20)
        {
            if(limit > 0)
                return await this.dbSet.Where(func).Take(limit).ToListAsync();
            return await this.dbSet.Where(func).ToListAsync();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var data = await GetAsync(x=>x.id == id);
            if(data != null)
            {
                this.SetClient(data);
                data.is_active = false;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order> SaveChangeAsync(Order idata)
        {
            if(idata.id != Guid.Empty)            
                return await this.UpdateAsync(idata);            
            else            
                return await this.CreateAsync(idata);            
        }

        public async Task<Order> UpdateAsync(Order idata)
        {
            var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
            if (data == null)
                throw new Exception($"Order id:{idata.id} is not found");
            idata = SetClient(idata);
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return data;
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.code descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            uid = data.id,
                            label = data.code,
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

        public PagedList<Order> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Order()
                        {
                            id = data.id,
                            code = data.code,
                            created = data.created,
                            updated = data.updated,
                            total = data.Order_Details.Sum(x=>x.price)                            
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.code, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Order>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }
    }
}