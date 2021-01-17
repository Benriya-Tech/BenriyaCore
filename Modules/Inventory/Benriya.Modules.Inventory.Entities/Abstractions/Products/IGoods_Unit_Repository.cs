using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Modules.Inventory.Entities.Abstractions
{
    public interface IGoods_Unit_Repository:IRepository
    {
        Task<Goods_Unit> CreateAsync(Goods_Unit idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Goods_Unit> GetAsync(Expression<Func<Goods_Unit, bool>> func);
        PagedList<Goods_Unit> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<Goods_Unit>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<Goods_Unit>> ListAsync(Expression<Func<Goods_Unit, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<Goods_Unit> SaveChangeAsync(Goods_Unit idata);
        Task<Goods_Unit> UpdateAsync(Goods_Unit idata);
    }
}