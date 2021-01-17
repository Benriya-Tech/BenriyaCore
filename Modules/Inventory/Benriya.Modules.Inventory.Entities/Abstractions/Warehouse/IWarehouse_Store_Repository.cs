using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Modules.Inventory.Entities.Abstractions
{
    public interface IWarehouse_Store_Repository:IRepository
    {
        Task<Warehouse_Store> CreateAsync(Warehouse_Store idata);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Warehouse_Store> GetAsync(Expression<Func<Warehouse_Store, bool>> func);
        PagedList<Warehouse_Store> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<List<Warehouse_Store>> ListAsync(Expression<Func<Warehouse_Store, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Warehouse_Store> SaveChangeAsync(Warehouse_Store idata);
        Task<Warehouse_Store> UpdateAsync(Warehouse_Store idata);
    }
}