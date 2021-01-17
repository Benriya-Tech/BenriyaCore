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
    public interface IGoods_Repository:IRepository
    {
        Task<Goods> CreateAsync(Goods idata);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<Goods> GetAsync(Expression<Func<Goods, bool>> func);
        PagedList<Goods> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<Goods>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<Goods>> ListAsync(Expression<Func<Goods, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(Guid id);
        Task<Goods> SaveChangeAsync(Goods idata);
        Task<Goods> UpdateAsync(Goods idata);
    }
}