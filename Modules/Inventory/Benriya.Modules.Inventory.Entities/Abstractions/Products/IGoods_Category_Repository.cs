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
    public interface IGoods_Category_Repository:IRepository
    {
        Task<Goods_Category> CreateAsync(Goods_Category idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Goods_Category> GetAsync(Expression<Func<Goods_Category, bool>> func);
        PagedList<Goods_Category> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<Goods_Category>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<Goods_Category>> ListAsync(Expression<Func<Goods_Category, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<Goods_Category> SaveChangeAsync(Goods_Category idata);
        Task<Goods_Category> UpdateAsync(Goods_Category idata);
    }
}