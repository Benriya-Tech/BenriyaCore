using System;
using Benriya.Share.Models.Menus;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Benriya.Utils.Extensions;
using Benriya.Share.ViewModels;
using Benriya.Core.Abstractions.Menus;
using System.Data;

namespace Benriya.Core.Repositories.Menus
{
    public class Menu_Repository : RepositoryBase<SystemMenu>, IMenu_Repository
    {
        private SystemMenu SetClient(SystemMenu idata, bool is_new = false)
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
        public async Task<SystemMenu> GetAsync(Expression<Func<SystemMenu, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);

        }


        public async Task<List<SystemMenu>> ListAsync(Expression<Func<SystemMenu, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<SystemMenu> CreateAsync(SystemMenu idata)
        {
            //try
            //{
            if (idata.name.isNOEOW() || idata.code.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            var u = await dbSet.FirstOrDefaultAsync(x => x.code == idata.code);
            if (u != null)
                throw new DuplicateNameException("[code]", new Exception($"Menu code is already used by existing object"));
            idata = SetClient(idata,true);
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
            //}catch(Exception e)
            //{
            //    throw;
            //}
        }


        public async Task<SystemMenu> UpdateAsync(SystemMenu idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                data = SetClient(data);
                data.name = idata.name;
                data.code = idata.code;
                data.url = idata.url;
                data.parent_menu_id = idata.parent_menu_id;
                data.description = idata.description;
                data.icon = idata.icon;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var client = Client;
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
                if (data == null)
                    throw new Exception($"Role id:{id} is not found");
                data = SetClient(data);                
                data.is_active = false;
                if (client.CurrentUser != null)
                    data.updated_by = client.CurrentUser.id;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<SystemMenu> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new SystemMenu()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
                            parent_menu_id = data.parent_menu_id,
                            code = data.code,
                            is_active = data.is_active,
                            created_by = data.created_by
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
            return new PagedList<SystemMenu>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync (Expression<Func<DropdownItem, bool>> func, int limit = 20)
        {
            var data = dbSet.Select(x => new DropdownItem() {
                label = x.name,
                value = x.id.ToString(),
                description = x.description,
                ref_code = x.parent_menu_id.ToString()
            }).Where(func).OrderBy(o => o.label);
            if(limit > 0)
                data.Take(limit);
            
            return await data.ToListAsync();
        }
    }
}
