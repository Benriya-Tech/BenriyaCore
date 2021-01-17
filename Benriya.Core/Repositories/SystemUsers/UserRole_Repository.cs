using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Models;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Benriya.Core.Abstractions.SystemUsers;
using System.Threading.Tasks;
using Benriya.Utils.Extensions;
using Benriya.Utils.Pagingation;

namespace Benriya.Core.Repositories.SystemUsers
{
    //[ScopedService]
    public class UserRole_Repository : RepositoryBase<User_Role>, IUserRole_Repository
    {
        public async Task<User_Role> GetAsync(Expression<Func<User_Role, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);

        }
        public async Task<List<User_Role>> ListAsync(Expression<Func<User_Role, bool>> func, int limit = 20)
        {
            return await this.dbSet.Where(func).OrderBy(o => o.name).Take(limit).ToListAsync();
        }

        public async Task<User_Role> CreateAsync(User_Role idata)
        {
            if (idata.name.isNOEOW() || idata.code.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            var u = await dbSet.FirstOrDefaultAsync(x => x.code == idata.code);
            if (u != null)
                throw new ArgumentException($"Code {idata.code} already exists");
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<User_Role> UpdateAsync(User_Role idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                data.name = idata.name;
                data.code = idata.code;
                data.description = idata.description;
                data.updated = DateTime.Now;
                data.updated_by = idata.updated_by;
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
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
                if (data == null)
                    throw new Exception($"Role id:{id} is not found");

                data.updated = DateTime.Now;
                data.is_active = false;
               data.updated_by = Client.CurrentUser.id;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new KeyNotFoundException($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<User_Role> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new User_Role()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
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
            return new PagedList<User_Role>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20)
        {
            return await dbSet.Select(x => new DropdownItem()
            {
                label = x.name,
                value = x.id.ToString(),
                description = x.description,
                ref_code = x.code
            }).Where(func).OrderBy(o => o.label).Take(limit).ToListAsync();
        }

    }
}
