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
    public interface IWarehouse_Repository:IRepository
    {
        Task<Warehouse> CreateAsync(Warehouse idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Warehouse> GetAsync(Expression<Func<Warehouse, bool>> func);
        PagedList<Warehouse> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<Warehouse>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<Warehouse>> ListAsync(Expression<Func<Warehouse, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<Warehouse> SaveChangeAsync(Warehouse idata);
        Task<Warehouse> UpdateAsync(Warehouse idata);
        Task<string> CheckUnique(Expression<Func<Warehouse, string>> field, string strSearch, string field_title = null);
        Task<Warehouse_Area> CheckArea(Warehouse_Area idata);
    }
}