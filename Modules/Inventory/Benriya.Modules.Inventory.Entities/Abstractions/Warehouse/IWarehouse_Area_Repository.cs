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
    public interface IWarehouse_Area_Repository:IRepository
    {
        Task<Warehouse_Area> CreateAsync(Warehouse_Area idata);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Warehouse_Area> GetAsync(Expression<Func<Warehouse_Area, bool>> func);
        PagedList<Warehouse_Area> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<Warehouse_Area>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<Warehouse_Area>> ListAsync(Expression<Func<Warehouse_Area, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Warehouse_Area> SaveChangeAsync(Warehouse_Area idata);
        Task<Warehouse_Area> UpdateAsync(Warehouse_Area idata);
    }
}