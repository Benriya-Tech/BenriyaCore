using Benriya.Share.Models.SystemUsers;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Extensions;
using Benriya.Utils.Pagingation;
using Benriya.Utils.Models;
using Benriya.Core.Abstractions.SystemUsers;

namespace Benriya.Core.Repositories.SystemUsers
{
    public class Permission_Repository : RepositoryBase<Permission_Access>, IPermission_Repository
    {
        private Permission_Access SetClient(Permission_Access idata, bool is_new = false)
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

        public async Task<Permission_Access> GetAsync(Expression<Func<Permission_Access, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }


        public async Task<List<Permission_Access>> ListAsync(Expression<Func<Permission_Access, bool>> func, int limit = 20)
        {
            var query = this.dbSet.Where(func);
            if (limit > 0)
                query = query.Take(limit);
            return await query.OrderBy(o => o.name).ToListAsync();
        }

        public async Task<Permission_Access> CreateAsync(Permission_Access idata)
        {
            if (idata.name.isNOEOW() || idata.code.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            var u = await dbSet.FirstOrDefaultAsync(x => x.code == idata.code);
            if (u != null)
                throw new ArgumentException($"Code {idata.code} already exists");
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Permission_Access> UpdateAsync(Permission_Access idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Permission id:{idata.id} is not found");
                data.name = idata.name;
                data.code = idata.code;
                data.is_active = idata.is_active;
                data = SetClient(data);
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
                    throw new Exception($"Permission id:{id} is not found");
                data = SetClient(data);
                data.is_active = false;
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
                throw new KeyNotFoundException($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Permission_Access> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Permission_Access()
                        {
                            id = data.id,
                            name = data.name,
                            code = data.code,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
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
                    EF.Functions.Like(x.code, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.description, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Permission_Access>(query, pagingParams.PageNumber, pagingParams.PageSize);
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
